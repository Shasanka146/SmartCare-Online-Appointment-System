using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartCare.Repository.Database;
using SmartCare.Shared.DoctorData;

namespace SmartCare.Repository.DoctorRepository;

public class doctorRepository(IConfiguration configuration, ILogger<doctorRepository> logger) : IdoctorRepository
{
    public async Task<List<DoctorDetails>> GetAllDoctors() => await GetDoctors("SELECT DoctorId, Name, Status FROM Doctors ORDER BY Name;", null);

    public async Task<List<DoctorDetails>> GetDoctorsByStatus(string status) => await GetDoctors("SELECT DoctorId, Name, Status FROM Doctors WHERE Status = @Status ORDER BY Name;", status);

    public Task<int> GetDoctorCount() => Count("SELECT COUNT(*) FROM Doctors;", null);

    public Task<int> GetDoctorCountByStatus(string status) => Count("SELECT COUNT(*) FROM Doctors WHERE Status = @Status;", status);

    private async Task<List<DoctorDetails>> GetDoctors(string sql, string? status)
    {
        var doctors = new List<DoctorDetails>();
        try
        {
            await using var connection = SqliteConnectionFactory.Create(configuration);
            await using var command = new SqliteCommand(sql, connection);
            if (status is not null) command.Parameters.AddWithValue("@Status", status);
            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync()) doctors.Add(new DoctorDetails { DoctorId = reader.GetInt32(0), Name = reader.GetString(1), Status = reader.GetString(2) });
        }
        catch (SqliteException exception) { logger.LogError(exception, "Unable to retrieve doctors."); }
        return doctors;
    }

    private async Task<int> Count(string sql, string? status)
    {
        try
        {
            await using var connection = SqliteConnectionFactory.Create(configuration);
            await using var command = new SqliteCommand(sql, connection);
            if (status is not null) command.Parameters.AddWithValue("@Status", status);
            await connection.OpenAsync();
            return Convert.ToInt32(await command.ExecuteScalarAsync());
        }
        catch (SqliteException exception) { logger.LogError(exception, "Unable to count doctors."); return 0; }
    }
}
