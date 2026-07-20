using Microsoft.EntityFrameworkCore;
using SmartCare.Repository.Database;
using SmartCare.Shared.DoctorData;

namespace SmartCare.Repository.DoctorRepository;

public class doctorRepository(ApplicationDbContext dbContext) : IdoctorRepository
{
    Task<List<DoctorDetails>> IdoctorRepository.GetAllDoctors() => dbContext.Doctors.AsNoTracking().ToListAsync();
    public Task<List<DoctorDetails>> GetDoctorsByStatus(string status) => dbContext.Doctors.AsNoTracking().Where(doctor => doctor.Status == status).ToListAsync();
}
