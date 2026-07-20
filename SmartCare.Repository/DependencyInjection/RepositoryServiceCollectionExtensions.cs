using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartCare.Repository.Database;
using SmartCare.Repository.PatientRepository;
using SmartCare.Repository.AppointmentRepository;
using SmartCare.Repository.DoctorRepository;

namespace SmartCare.Repository.DependencyInjection;

public static class RepositoryServiceCollectionExtensions
{
    public static IServiceCollection AddSmartCareRepository(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Use one deterministic SQLite location for startup initialization and
        // request scopes, regardless of the current working directory.
        var databasePath = ResolveDatabasePath(configuration);
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite($"Data Source={databasePath}"));

        services.AddScoped<IPatientRepository, patientRepository>();
        services.AddScoped<IAppointmentRepository, global::SmartCare.Repository.AppointmentRepository.AppointmentRepository>();
        services.AddScoped<IdoctorRepository, doctorRepository>();

        return services;
    }

    public static string ResolveDatabasePath(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ApplicationDbContext");
        if (string.IsNullOrWhiteSpace(connectionString))
            return Path.Combine(AppContext.BaseDirectory, "smartcare.db");

        const string dataSourcePrefix = "Data Source=";
        if (connectionString.StartsWith(dataSourcePrefix, StringComparison.OrdinalIgnoreCase))
        {
            var dataSource = connectionString[dataSourcePrefix.Length..].Trim();
            if (Path.IsPathRooted(dataSource))
                return dataSource;

            return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, dataSource));
        }

        return connectionString;
    }

    public static void EnsureSmartCareDatabase(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.EnsureCreated();
    }
}
