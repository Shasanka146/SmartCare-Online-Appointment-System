using SmartCare.Business.DependencyInjection;
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
