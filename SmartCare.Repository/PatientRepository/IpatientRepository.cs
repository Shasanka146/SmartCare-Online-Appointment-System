using SmartCare.Shared.PatientData;

namespace SmartCare.Repository.PatientRepository
{
    public interface IPatientRepository
    {
        Task<bool> AddPatient(PatientDetails patient);
        Task<PatientDetails> GetPatientById(int id);
        Task<List<PatientDetails>> GetAllPatients();
        Task<bool> UpdatePatient(PatientDetails patient);
    }
}
