using Microsoft.AspNetCore.Mvc;
using SmartCare.Business.ClinicBusiness;
using SmartCare.Shared.ClinicData;
using System.Threading.Tasks;

namespace SmartCare.Main.Controllers
{
    public class ClinicController : Controller
    {
        private readonly IClinicBusiness _business;

        public ClinicController(IClinicBusiness business)
        {
            _business = business;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _business.GetAllClinics();
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClinicDetails clinic)
        {
            if (ModelState.IsValid)
            {
                await _business.AddClinic(clinic);
                return RedirectToAction("Index");
            }
            return View(clinic);
        }
    }
}