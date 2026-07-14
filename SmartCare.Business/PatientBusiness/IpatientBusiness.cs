using System.Collections.Generic;
using System.Threading.Tasks;
using SmartCare.Shared.PatientData;

namespace SmartCare.Business.PatientBusiness
{
    public interface IpatientBusiness
    {
        Task<bool> AddPatient(PatientDetails patient);
        Task<PatientDetails?> GetPatientById(int id);
        Task<List<PatientDetails>> GetAllPatients();
        Task<bool> UpdatePatient(PatientDetails patient);
    }
}
