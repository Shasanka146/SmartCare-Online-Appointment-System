using SmartCare.Repository.DoctorRepository;
using SmartCare.Shared.DoctorData;

namespace SmartCare.Business.DoctorBusiness;

public class doctorBusiness(IdoctorRepository doctorRepository) : IdoctorBusiness
{
    public Task<List<DoctorDetails>> GetAllDoctors() => doctorRepository.GetAllDoctors();

    public Task<List<DoctorDetails>> GetDoctorsByStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status)) throw new ArgumentException("Status is required.", nameof(status));
        return doctorRepository.GetDoctorsByStatus(status.Trim());
    }

    public Task<int> GetDoctorCount() => doctorRepository.GetDoctorCount();

    public Task<int> GetDoctorCountByStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status)) throw new ArgumentException("Status is required.", nameof(status));
        return doctorRepository.GetDoctorCountByStatus(status.Trim());
    }
}
