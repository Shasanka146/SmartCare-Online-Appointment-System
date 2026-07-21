using SmartCare.Shared.ClinicData;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartCare.Repository.ClinicRepository
{
    public interface IClinicRepository
    {
        Task<bool> AddClinic(ClinicDetails clinic);
        Task<List<ClinicDetails>> GetAllClinics();
        Task<ClinicDetails?> GetClinicById(int clinicId);
        Task<bool> UpdateClinic(ClinicDetails clinic);
        Task<bool> DeleteClinic(int clinicId);
        Task<int> GetClinicCount();
    }
}
