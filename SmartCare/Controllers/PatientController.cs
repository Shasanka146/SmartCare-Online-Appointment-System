using Microsoft.AspNetCore.Mvc;
using SmartCare.Business.PatientBusiness;
using SmartCare.Shared.PatientData;

namespace SmartCare.Controllers;

[ApiController]
[Route("api/patients")]
public class PatientController : ControllerBase
{
    private readonly IpatientBusiness _patientBusiness;

    public IpatientBusiness PatientBusiness => _patientBusiness;

    public PatientController(IpatientBusiness patientBusiness) => _patientBusiness = patientBusiness;

    [HttpGet]
    public async Task<ActionResult<List<PatientDetails>>> GetAll() => Ok(await PatientBusiness.GetAllPatients());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PatientDetails>> GetById(int id)
    {
        var patient = await PatientBusiness.GetPatientById(id);
        return patient is null ? NotFound() : Ok(patient);
    }

    [HttpPost]
    public async Task<ActionResult<PatientDetails>> Create(PatientDetails patient)
    {
        if (!await PatientBusiness.AddPatient(patient))
            return BadRequest("The patient could not be created.");

        return CreatedAtAction(nameof(GetById), new { id = patient.PatientId }, patient);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, PatientDetails patient)
    {
        if (id != patient.PatientId)
            return BadRequest("The route id must match PatientId.");

        return await PatientBusiness.UpdatePatient(patient) ? NoContent() : NotFound();
    }
}
