using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartCare.Business.AppointmentBusiness;
using SmartCare.Business.ClinicBusiness;
using SmartCare.Business.DoctorBusiness;
using SmartCare.Business.PatientBusiness;
using SmartCare.Repository.DependencyInjection;
using AppointmentService = SmartCare.Business.AppointmentBusiness.AppointmentBusiness;

namespace SmartCare.Business.DependencyInjection;

public static class BusinessServiceCollectionExtensions
{
    public static IServiceCollection AddSmartCareServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSmartCareRepository(configuration);

        services.AddScoped<IAppointmentBusiness, AppointmentService>();
        services.AddScoped<IdoctorBusiness, doctorBusiness>();
        services.AddScoped<IpatientBusiness, patientBusiness>();
        services.AddScoped<IclinicBusiness, clinicBusiness>();

        return services;
    }

    public static void EnsureSmartCareDatabase(this IServiceProvider serviceProvider) =>
        RepositoryServiceCollectionExtensions.EnsureSmartCareDatabase(serviceProvider);
}
