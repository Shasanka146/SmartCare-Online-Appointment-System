using Microsoft.EntityFrameworkCore;
using SmartCare.Repository.Database;
using SmartCare.Shared.PatientData;

namespace SmartCare.Repository.PatientRepository;

public class patientRepository : IPatientRepository
{
    private readonly ApplicationDbContext _dbContext;

    public patientRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<bool> AddPatient(PatientDetails patient)
    {
        await _dbContext.Patients.AddAsync(patient);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public Task<List<PatientDetails>> GetAllPatients() => _dbContext.Patients
        .AsNoTracking()
        .OrderBy(patient => patient.Name)
        .ToListAsync();

    public Task<PatientDetails?> GetPatientById(int id) => _dbContext.Patients
        .AsNoTracking()
        .FirstOrDefaultAsync(patient => patient.PatientId == id);

    public async Task<bool> UpdatePatient(PatientDetails patient)
    {
        var existingPatient = await _dbContext.Patients.FindAsync(patient.PatientId);
        if (existingPatient is null) return false;

        existingPatient.Name = patient.Name;
        existingPatient.UserId = patient.UserId;
        existingPatient.Age = patient.Age;
        existingPatient.Gender = patient.Gender;
        existingPatient.Contactno = patient.Contactno;
        existingPatient.Address = patient.Address;

        return await _dbContext.SaveChangesAsync() > 0;
    }
}
