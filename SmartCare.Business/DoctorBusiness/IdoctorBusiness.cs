using SmartCare.Shared.DoctorData;

namespace SmartCare.Business.DoctorBusiness;

public interface IdoctorBusiness
{
    Task<List<DoctorDetails>> GetAllDoctors();
    Task<List<DoctorDetails>> GetDoctorsByStatus(string status);
}
