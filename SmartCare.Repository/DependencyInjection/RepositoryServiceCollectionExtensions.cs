using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartCare.Repository.Database;
using SmartCare.Repository.PatientRepository;
using SmartCare.Repository.AppointmentRepository;

namespace SmartCare.Repository.DependencyInjection;

public static class RepositoryServiceCollectionExtensions
{
    public static IServiceCollection AddSmartCareRepository(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("ApplicationDbContext")));

        services.AddScoped<IPatientRepository, patientRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();

        return services;
    }
    public static void EnsureSmartCareDatabase(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.EnsureCreated();
    }
}
