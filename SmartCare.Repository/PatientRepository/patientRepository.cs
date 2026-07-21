using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartCare.Repository.Database;
using SmartCare.Shared.PatientData;

namespace SmartCare.Repository.PatientRepository;

public class patientRepository(IConfiguration configuration, ILogger<patientRepository> logger) : IPatientRepository
{
    public async Task<bool> AddPatient(PatientDetails patient)
    {
        const string sql = "INSERT INTO Patients (Name, UserId, Age, Gender, Contactno, Address, CreatedDate) VALUES (@Name, @UserId, @Age, @Gender, @Contactno, @Address, @CreatedDate); SELECT last_insert_rowid();";
        try
        {
            await using var connection = SqliteConnectionFactory.Create(configuration);
            await using var command = new SqliteCommand(sql, connection);
            AddPatientParameters(command, patient);
            await connection.OpenAsync();
            patient.PatientId = Convert.ToInt32(await command.ExecuteScalarAsync());
            return patient.PatientId > 0;
        }
        catch (SqliteException exception)
        {
            logger.LogError(exception, "Unable to add patient.");
            return false;
        }
    }

    public async Task<List<PatientDetails>> GetAllPatients()
    {
        var patients = new List<PatientDetails>();
        try
        {
            await using var connection = SqliteConnectionFactory.Create(configuration);
            await using var command = new SqliteCommand("SELECT PatientId, Name, UserId, Age, Gender, Contactno, Address, CreatedDate FROM Patients ORDER BY Name;", connection);
            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync()) patients.Add(MapPatient(reader));
        }
        catch (SqliteException exception) { logger.LogError(exception, "Unable to retrieve patients."); }
        return patients;
    }

    public async Task<PatientDetails?> GetPatientById(int id)
    {
        try
        {
            await using var connection = SqliteConnectionFactory.Create(configuration);
            await using var command = new SqliteCommand("SELECT PatientId, Name, UserId, Age, Gender, Contactno, Address, CreatedDate FROM Patients WHERE PatientId = @Id;", connection);
            command.Parameters.AddWithValue("@Id", id);
            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapPatient(reader) : null;
        }
        catch (SqliteException exception) { logger.LogError(exception, "Unable to retrieve patient {PatientId}.", id); return null; }
    }

    public async Task<int> GetPatientCount()
    {
        try
        {
            await using var connection = SqliteConnectionFactory.Create(configuration);
            await using var command = new SqliteCommand("SELECT COUNT(*) FROM Patients;", connection);
            await connection.OpenAsync();
            return Convert.ToInt32(await command.ExecuteScalarAsync());
        }
        catch (SqliteException exception) { logger.LogError(exception, "Unable to count patients."); return 0; }
    }

    public async Task<bool> UpdatePatient(PatientDetails patient)
    {
        const string sql = "UPDATE Patients SET Name=@Name, UserId=@UserId, Age=@Age, Gender=@Gender, Contactno=@Contactno, Address=@Address WHERE PatientId=@PatientId;";
        try
        {
            await using var connection = SqliteConnectionFactory.Create(configuration);
            await using var command = new SqliteCommand(sql, connection);
            AddPatientParameters(command, patient);
            command.Parameters.AddWithValue("@PatientId", patient.PatientId);
            await connection.OpenAsync();
            return await command.ExecuteNonQueryAsync() > 0;
        }
        catch (SqliteException exception) { logger.LogError(exception, "Unable to update patient {PatientId}.", patient.PatientId); return false; }
    }

    private static void AddPatientParameters(SqliteCommand command, PatientDetails patient)
    {
        command.Parameters.AddWithValue("@Name", patient.Name);
        command.Parameters.AddWithValue("@UserId", patient.UserId);
        command.Parameters.AddWithValue("@Age", patient.Age);
        command.Parameters.AddWithValue("@Gender", patient.Gender);
        command.Parameters.AddWithValue("@Contactno", patient.Contactno);
        command.Parameters.AddWithValue("@Address", patient.Address);
        command.Parameters.AddWithValue("@CreatedDate", patient.CreatedDate.ToString("O"));
    }

    private static PatientDetails MapPatient(SqliteDataReader reader) => new()
    {
        PatientId = reader.GetInt32(0), Name = reader.GetString(1), UserId = reader.GetInt32(2), Age = reader.GetInt32(3),
        Gender = reader.GetString(4), Contactno = reader.GetString(5), Address = reader.GetString(6),
        CreatedDate = DateTime.TryParse(reader.GetString(7), out var createdDate) ? createdDate : DateTime.UtcNow
    };
}
