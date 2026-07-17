using SmartCare.Shared.ClinicData;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartCare.Business.ClinicBusiness
{
    public interface IClinicBusiness
    {
        Task<bool> AddClinic(ClinicDetails clinic);
        Task<List<ClinicDetails>> GetAllClinics();
        Task<ClinicDetails> GetClinicById(int clinicId);
        Task<bool> UpdateClinic(ClinicDetails clinic);
        Task<bool> DeleteClinic(int clinicId);
    }
}