using SmartCare.Business.AppointmentBusiness;
using SmartCare.Business.ClinicBusiness;
using SmartCare.Business.DependencyInjection;
using SmartCare.Business.PatientBusiness;
using SmartCare.Repository.AppointmentRepository;
using SmartCare.Repository.ClinicRepository;
using SmartCare.Repository.PatientRepository;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.DataProtection;

public partial class Program
{
    private static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        // Avoid Windows-profile key access in this shared development workspace.
        builder.Services.AddSingleton<IDataProtectionProvider, EphemeralDataProtectionProvider>();
        builder.Services.AddSmartCareServices(builder.Configuration);

        builder.Services.AddScoped<IpatientBusiness, patientBusiness>();
        builder.Services.AddScoped<IPatientRepository, patientRepository>();

        builder.Services.AddScoped<IClinicBusiness, global::SmartCare.Business.ClinicBusiness.ClinicBusiness>();
        builder.Services.AddScoped<IClinicRepository>(_ => new global::SmartCare.Repository.ClinicRepository.ClinicRepository(
            builder.Configuration.GetConnectionString("ApplicationDbContext")!));

        builder.Services.AddScoped<IAppointmentBusiness, AppointmentBusiness>();
        builder.Services.AddScoped<IAppointmentRepository, global::SmartCare.Repository.AppointmentRepository.IAppointmentRepository>();

        var app = builder.Build();
        app.Services.EnsureSmartCareDatabase();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        // Host the standalone dashboard from this application so it can call
        // /api/patients without requiring a separate CORS setup.
        var frontendPath = Path.Combine(app.Environment.ContentRootPath, "frontend");
        if (!Directory.Exists(frontendPath))
            frontendPath = Path.GetFullPath(Path.Combine(app.Environment.ContentRootPath, "..", "frontend"));
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(frontendPath),
            RequestPath = "/frontend"
        });
        app.UseRouting();

        app.UseAuthorization();

        app.MapControllers();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");


        app.Run();
    }
}
