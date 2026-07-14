using Microsoft.AspNetCore.Mvc;
using SmartCare.Business.AppointmentBusiness;
using SmartCare.Shared.AppointmentData;

namespace SmartCare.Controllers;

public class AppointmentController : Controller
{
    private readonly IAppointmentBusiness _appointmentBusiness;

    public AppointmentController(IAppointmentBusiness appointmentBusiness) =>
        _appointmentBusiness = appointmentBusiness;

    public async Task<IActionResult> Index()
    {
        var appointments = await _appointmentBusiness.GetAllAppointments();
        return View(appointments);
    }

    [HttpGet]
    public IActionResult Create() => View(new AppointmentDetails
    {
        AppointmentDate = DateTime.Now.AddHours(1),
        Status = "Scheduled"
    });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AppointmentDetails appointment)
    {
        if (!ModelState.IsValid) return View(appointment);

        try
        {
            if (!await _appointmentBusiness.AddAppointment(appointment))
            {
                ModelState.AddModelError(string.Empty, "Appointment could not be saved.");
                return View(appointment);
            }
        }
        catch (ArgumentException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return View(appointment);
        }

        TempData["SuccessMessage"] = "Appointment created successfully.";
        return RedirectToAction(nameof(Index));
    }
}
