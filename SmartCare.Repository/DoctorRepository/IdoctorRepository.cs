using SmartCare.Shared.DoctorData;

namespace SmartCare.Repository.DoctorRepository;

public interface IdoctorRepository
{
    Task<List<DoctorDetails>> GetAllDoctors();
    Task<List<DoctorDetails>> GetDoctorsByStatus(string status);
    Task<int> GetDoctorCount();
    Task<int> GetDoctorCountByStatus(string status);
}
