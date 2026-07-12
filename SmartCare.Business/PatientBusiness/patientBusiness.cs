using SmartCare.Repository.PatientRepository;
using SmartCare.Repository.Models;
using SmartCare.Shared.PatientData;


namespace SmartCare.Business.PatientBusiness
{
    public class patientBusiness : IpatientBusiness
    {
        public Task<bool> AddPatient(PatientDetails patient)
        {
            throw new NotImplementedException();
        }

        public Task<List<PatientDetails>> GetAllPatients()
        {
            throw new NotImplementedException();
        }

        public Task<PatientDetails> GetPatientById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePatient(PatientDetails patient)
        {
            throw new NotImplementedException();
        }
    }
}
