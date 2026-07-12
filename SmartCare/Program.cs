using SmartCare.Business.AppointmentBusiness;
using SmartCare.Business.DoctorBusiness;
using SmartCare.Business.PatientBusiness;
using SmartCare.Business.ClinicBusiness;
using SmartCare.Repository.AppointmentRepository;
using SmartCare.Repository.DoctorRepository;
using SmartCare.Repository.PatientRepository;
using SmartCare.Repository.ClinicRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

public partial class Program
{
    private static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddScoped<IappointmentBusiness, appointmentBusiness>();
        builder.Services.AddScoped<IappointmentRepository, appointmentRepository>();

        builder.Services.AddScoped<IdoctorBusiness, doctorBusiness>();
        builder.Services.AddScoped<IdoctorRepository, doctorRepository>();

        builder.Services.AddScoped<IpatientBusiness, patientBusiness>();
        builder.Services.AddScoped<IPatientRepository, patientRepository>();

        builder.Services.AddScoped<IclinicBusiness, clinicBusiness>();
        builder.Services.AddScoped<IclinicRepository, clinicRepository>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthorization();

        app.MapStaticAssets();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();


        app.Run();
    }
}