using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartCare.Repository.PatientRepository;
using SmartCare.Repository.AppointmentRepository;
using SmartCare.Repository.DoctorRepository;
using SmartCare.Repository.ClinicRepository;

namespace SmartCare.Repository.DependencyInjection;

public static class RepositoryServiceCollectionExtensions
{
    public static IServiceCollection AddSmartCareRepository(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IPatientRepository, patientRepository>();
        services.AddScoped<IAppointmentRepository, global::SmartCare.Repository.AppointmentRepository.AppointmentRepository>();
        services.AddScoped<IdoctorRepository, doctorRepository>();
        services.AddScoped<IClinicRepository, global::SmartCare.Repository.ClinicRepository.ClinicRepository>();

        return services;
    }

    public static void EnsureSmartCareDatabase(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        using var connection = new SqliteConnection(Database.SqliteConnectionFactory.GetConnectionString(configuration));
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = """
            CREATE TABLE IF NOT EXISTS Patients (
                PatientId INTEGER NOT NULL CONSTRAINT PK_Patients PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                UserId INTEGER NOT NULL,
                Age INTEGER NOT NULL,
                Gender TEXT NOT NULL,
                Contactno TEXT NOT NULL,
                Address TEXT NOT NULL,
                CreatedDate TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Clinics (
                ClinicId INTEGER NOT NULL CONSTRAINT PK_Clinics PRIMARY KEY AUTOINCREMENT,
                ClinicName TEXT NOT NULL,
                Address TEXT NOT NULL,
                PhoneNumber TEXT NOT NULL,
                Email TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Doctors (
                DoctorId INTEGER NOT NULL CONSTRAINT PK_Doctors PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Status TEXT NOT NULL DEFAULT 'Available'
            );

            CREATE TABLE IF NOT EXISTS Appointments (
                AppointmentId INTEGER NOT NULL CONSTRAINT PK_Appointments PRIMARY KEY AUTOINCREMENT,
                PatientId INTEGER NOT NULL,
                DoctorId INTEGER NOT NULL,
                AppointmentDate TEXT NOT NULL,
                Status TEXT NOT NULL
            );
            """;
        command.ExecuteNonQuery();
        EnsureColumn(connection, "Patients", "CreatedDate", "ALTER TABLE Patients ADD COLUMN CreatedDate TEXT NOT NULL DEFAULT ''; ");
        EnsureColumn(connection, "Doctors", "Status", "ALTER TABLE Doctors ADD COLUMN Status TEXT NOT NULL DEFAULT 'Available';");
    }

    private static void EnsureColumn(SqliteConnection connection, string tableName, string columnName, string alterSql)
    {
        using var tableInfo = connection.CreateCommand();
        tableInfo.CommandText = $"PRAGMA table_info({tableName});";
        using var reader = tableInfo.ExecuteReader();
        var exists = false;
        while (reader.Read())
            exists |= string.Equals(reader.GetString(1), columnName, StringComparison.OrdinalIgnoreCase);

        reader.Close();
        if (exists) return;

        using var alter = connection.CreateCommand();
        alter.CommandText = alterSql;
        alter.ExecuteNonQuery();
    }
}
