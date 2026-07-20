using Microsoft.EntityFrameworkCore;
using SmartCare.Repository.Database;
using SmartCare.Shared.AppointmentData;

namespace SmartCare.Repository.AppointmentRepository;

public class AppointmentRepository(ApplicationDbContext dbContext) : IAppointmentRepository
{
    public async Task<bool> AddAppointment(AppointmentDetails appointment)
    {
        await dbContext.Appointments.AddAsync(appointment);
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<IReadOnlyList<AppointmentDetails>> GetAllAppointments() =>
        await dbContext.Appointments.AsNoTracking().OrderBy(appointment => appointment.AppointmentDate).ToListAsync();

    public async Task<IReadOnlyList<AppointmentDetails>> GetAppointmentsByPatient(int patientId) =>
        await dbContext.Appointments.AsNoTracking().Where(appointment => appointment.PatientId == patientId).OrderBy(appointment => appointment.AppointmentDate).ToListAsync();

    public async Task<bool> UpdateStatus(int appointmentId, string status)
    {
        var appointment = await dbContext.Appointments.FindAsync(appointmentId);
        if (appointment is null) return false;
        appointment.Status = status;
        return await dbContext.SaveChangesAsync() > 0;
    }
}
