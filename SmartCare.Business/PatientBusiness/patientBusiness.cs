using SmartCare.Repository.PatientRepository;
using SmartCare.Shared.PatientData;

namespace SmartCare.Business.PatientBusiness;

public class patientBusiness : IpatientBusiness
{
    private readonly IPatientRepository _patientRepository;

    public patientBusiness(IPatientRepository patientRepository) => _patientRepository = patientRepository;

    public Task<bool> AddPatient(PatientDetails patient)
    {
        ArgumentNullException.ThrowIfNull(patient);
        return _patientRepository.AddPatient(patient);
    }

    public Task<List<PatientDetails>> GetAllPatients() => _patientRepository.GetAllPatients();

    public Task<PatientDetails?> GetPatientById(int id) => _patientRepository.GetPatientById(id);

    public Task<int> GetPatientCount() => _patientRepository.GetPatientCount();

    public Task<bool> UpdatePatient(PatientDetails patient)
    {
        ArgumentNullException.ThrowIfNull(patient);
        return _patientRepository.UpdatePatient(patient);
    }

    public async Task<List<PatientDetails>> GetNewPatients(DateTime date) =>
        (await _patientRepository.GetAllPatients())
            .Where(patient => patient.CreatedDate.Date == date.Date)
            .ToList();
}
