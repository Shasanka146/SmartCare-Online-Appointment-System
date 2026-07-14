using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SmartCare.Shared.AppointmentData;

namespace SmartCare.Repository.Appointment;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly string _connectionString;

    public AppointmentRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SmartCareConnection")
            ?? throw new InvalidOperationException("Connection string 'SmartCareConnection' was not found.");
    }

    public async Task<bool> AddAppointment(AppointmentDetails appointment)
    {
        const string sql = """
            INSERT INTO dbo.Appointments (PatientId, DoctorId, AppointmentDate, Status)
            VALUES (@PatientId, @DoctorId, @AppointmentDate, @Status);
            """;

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@PatientId", appointment.PatientId);
        command.Parameters.AddWithValue("@DoctorId", appointment.DoctorId);
        command.Parameters.AddWithValue("@AppointmentDate", appointment.AppointmentDate);
        command.Parameters.AddWithValue("@Status", appointment.Status);

        await connection.OpenAsync();
        return await command.ExecuteNonQueryAsync() == 1;
    }

    public Task<IReadOnlyList<AppointmentDetails>> GetAllAppointments() =>
        GetAppointments("SELECT AppointmentId, PatientId, DoctorId, AppointmentDate, Status FROM dbo.Appointments ORDER BY AppointmentDate;");

    public Task<IReadOnlyList<AppointmentDetails>> GetAppointmentsByPatient(int patientId) =>
        GetAppointments(
            "SELECT AppointmentId, PatientId, DoctorId, AppointmentDate, Status FROM dbo.Appointments WHERE PatientId = @PatientId ORDER BY AppointmentDate;",
            patientId);

    public async Task<bool> UpdateStatus(int appointmentId, string status)
    {
        const string sql = "UPDATE dbo.Appointments SET Status = @Status WHERE AppointmentId = @AppointmentId;";

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@AppointmentId", appointmentId);
        command.Parameters.AddWithValue("@Status", status);

        await connection.OpenAsync();
        return await command.ExecuteNonQueryAsync() == 1;
    }

    private async Task<IReadOnlyList<AppointmentDetails>> GetAppointments(string sql, int? patientId = null)
    {
        var appointments = new List<AppointmentDetails>();

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        if (patientId.HasValue)
            command.Parameters.AddWithValue("@PatientId", patientId.Value);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            appointments.Add(new AppointmentDetails
            {
                AppointmentId = reader.GetInt32(0),
                PatientId = reader.GetInt32(1),
                DoctorId = reader.GetInt32(2),
                AppointmentDate = reader.GetDateTime(3),
                Status = reader.GetString(4)
            });
        }

        return appointments;
    }
}
