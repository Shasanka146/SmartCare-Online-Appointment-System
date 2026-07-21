using Microsoft.AspNetCore.Mvc;
using SmartCare.Models;
using SmartCare.Business.ClinicBusiness;
using SmartCare.Business.DoctorBusiness;
using SmartCare.Business.PatientBusiness;
using System.Diagnostics;

namespace SmartCare.Controllers
{
    public class HomeController : Controller
    {
        private readonly IClinicBusiness _clinicBusiness;
        private readonly IdoctorBusiness _doctorBusiness;
        private readonly IpatientBusiness _patientBusiness;

        public HomeController(
            IClinicBusiness clinicBusiness,
            IdoctorBusiness doctorBusiness,
            IpatientBusiness patientBusiness)
        {
            _clinicBusiness = clinicBusiness;
            _doctorBusiness = doctorBusiness;
            _patientBusiness = patientBusiness;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["TotalClinics"] = await _clinicBusiness.GetClinicCount();
            ViewData["TotalDoctors"] = await _doctorBusiness.GetDoctorCount();
            ViewData["TotalPatients"] = await _patientBusiness.GetPatientCount();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
