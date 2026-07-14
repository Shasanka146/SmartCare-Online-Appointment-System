using SmartCare.Shared.AppointmentData;

namespace SmartCare.Business.AppointmentBusiness;

public interface IAppointmentBusiness
{
    Task<bool> AddAppointment(AppointmentDetails appointment);
    Task<IReadOnlyList<AppointmentDetails>> GetAllAppointments();
    Task<IReadOnlyList<AppointmentDetails>> GetAppointmentsByPatient(int patientId);
    Task<bool> UpdateStatus(int appointmentId, string status);
}
