using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartCare.Repository.Database;
using SmartCare.Shared.AppointmentData;

namespace SmartCare.Repository.AppointmentRepository;

public class AppointmentRepository(IConfiguration configuration, ILogger<AppointmentRepository> logger) : IAppointmentRepository
{
    public async Task<bool> AddAppointment(AppointmentDetails appointment)
    {
        const string sql = "INSERT INTO Appointments (PatientId, DoctorId, AppointmentDate, Status) VALUES (@PatientId, @DoctorId, @AppointmentDate, @Status); SELECT last_insert_rowid();";
        try
        {
            await using var connection = SqliteConnectionFactory.Create(configuration);
            await using var command = new SqliteCommand(sql, connection);
            AddParameters(command, appointment);
            await connection.OpenAsync();
            appointment.AppointmentId = Convert.ToInt32(await command.ExecuteScalarAsync());
            return appointment.AppointmentId > 0;
        }
        catch (SqliteException exception) { logger.LogError(exception, "Unable to add appointment."); return false; }
    }

    public Task<IReadOnlyList<AppointmentDetails>> GetAllAppointments() => GetAppointments("SELECT AppointmentId, PatientId, DoctorId, AppointmentDate, Status FROM Appointments ORDER BY AppointmentDate;", null);

    public Task<IReadOnlyList<AppointmentDetails>> GetAppointmentsByPatient(int patientId) => GetAppointments("SELECT AppointmentId, PatientId, DoctorId, AppointmentDate, Status FROM Appointments WHERE PatientId = @PatientId ORDER BY AppointmentDate;", patientId);

    public async Task<int> GetTodayAppointmentCount(DateTime date)
    {
        const string sql = "SELECT COUNT(*) FROM Appointments WHERE date(AppointmentDate) = @Date;";
        try
        {
            await using var connection = SqliteConnectionFactory.Create(configuration);
            await using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));
            await connection.OpenAsync();
            return Convert.ToInt32(await command.ExecuteScalarAsync());
        }
        catch (SqliteException exception) { logger.LogError(exception, "Unable to count today's appointments."); return 0; }
    }

    public async Task<bool> UpdateStatus(int appointmentId, string status)
    {
        try
        {
            await using var connection = SqliteConnectionFactory.Create(configuration);
            await using var command = new SqliteCommand("UPDATE Appointments SET Status = @Status WHERE AppointmentId = @AppointmentId;", connection);
            command.Parameters.AddWithValue("@Status", status);
            command.Parameters.AddWithValue("@AppointmentId", appointmentId);
            await connection.OpenAsync();
            return await command.ExecuteNonQueryAsync() > 0;
        }
        catch (SqliteException exception) { logger.LogError(exception, "Unable to update appointment {AppointmentId}.", appointmentId); return false; }
    }

    private async Task<IReadOnlyList<AppointmentDetails>> GetAppointments(string sql, int? patientId)
    {
        var appointments = new List<AppointmentDetails>();
        try
        {
            await using var connection = SqliteConnectionFactory.Create(configuration);
            await using var command = new SqliteCommand(sql, connection);
            if (patientId.HasValue) command.Parameters.AddWithValue("@PatientId", patientId.Value);
            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync()) appointments.Add(new AppointmentDetails { AppointmentId = reader.GetInt32(0), PatientId = reader.GetInt32(1), DoctorId = reader.GetInt32(2), AppointmentDate = DateTime.TryParse(reader.GetString(3), out var appointmentDate) ? appointmentDate : DateTime.MinValue, Status = reader.GetString(4) });
        }
        catch (SqliteException exception) { logger.LogError(exception, "Unable to retrieve appointments."); }
        return appointments;
    }

    private static void AddParameters(SqliteCommand command, AppointmentDetails appointment)
    {
        command.Parameters.AddWithValue("@PatientId", appointment.PatientId);
        command.Parameters.AddWithValue("@DoctorId", appointment.DoctorId);
        command.Parameters.AddWithValue("@AppointmentDate", appointment.AppointmentDate.ToString("O"));
        command.Parameters.AddWithValue("@Status", appointment.Status);
    }
}
