using Microsoft.AspNetCore.Mvc;
using SmartCare.Business.AppointmentBusiness;
using SmartCare.Business.PatientBusiness;
using SmartCare.Business.DoctorBusiness;
using SmartCare.Business.ClinicBusiness;
using SmartCare.Models;

namespace SmartCare.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IpatientBusiness _patientBusiness;
    private readonly IAppointmentBusiness _appointmentBusiness;
    private readonly IdoctorBusiness _doctorBusiness;
    private readonly IClinicBusiness _clinicBusiness;

    public DashboardController(
        IpatientBusiness patientBusiness,
        IAppointmentBusiness appointmentBusiness,
        IdoctorBusiness doctorBusiness,
        IClinicBusiness clinicBusiness)
    {
        _patientBusiness = patientBusiness;
        _appointmentBusiness = appointmentBusiness;
        _doctorBusiness = doctorBusiness;
        _clinicBusiness = clinicBusiness;
    }

    [HttpGet("stats")]
    public async Task<ActionResult<DashboardStats>> GetStats()
    {
        var totalPatients = await _patientBusiness.GetPatientCount();
        var totalDoctors = await _doctorBusiness.GetDoctorCount();
        var totalClinics = await _clinicBusiness.GetClinicCount();
        var todayAppointments = await _appointmentBusiness.GetTodayAppointmentCount(DateTime.Today);
        var availableDoctors = await _doctorBusiness.GetDoctorCountByStatus("Available");
        var busyDoctors = await _doctorBusiness.GetDoctorCountByStatus("Busy");

        return Ok(new DashboardStats
        {
            TotalDoctors = totalDoctors,
            TotalPatients = totalPatients,
            TotalClinics = totalClinics,
            TodayAppointments = todayAppointments,
            AvailableDoctors = availableDoctors,
            BusyDoctors = busyDoctors
        });
    }
}
