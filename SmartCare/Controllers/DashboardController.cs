using Microsoft.AspNetCore.Mvc;
using SmartCare.Business.AppointmentBusiness;
using SmartCare.Business.PatientBusiness;

namespace SmartCare.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IpatientBusiness _patientBusiness;
    private readonly IAppointmentBusiness _appointmentBusiness;

    public DashboardController(
        IpatientBusiness patientBusiness,
        IAppointmentBusiness appointmentBusiness)
    {
        _patientBusiness = patientBusiness;
        _appointmentBusiness = appointmentBusiness;
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var patients = await _patientBusiness.GetAllPatients();
        var appointments = await _appointmentBusiness.GetAllAppointments();

        return Ok(new
        {
            totalDoctors = 0,
            totalPatients = patients.Count,
            todayAppointments = appointments.Count(appointment =>
                appointment.AppointmentDate.Date == DateTime.Today),
            availableDoctors = 0,
            busyDoctors = 0,
            // PatientDetails currently has no CreatedDate property.
            newPatients = 0
        });
    }
}
