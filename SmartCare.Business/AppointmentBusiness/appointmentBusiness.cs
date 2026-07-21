using SmartCare.Repository.AppointmentRepository;
using SmartCare.Shared.AppointmentData;

namespace SmartCare.Business.AppointmentBusiness;

public class AppointmentBusiness : IAppointmentBusiness
{
    private readonly IAppointmentRepository _appointmentRepository;

    public AppointmentBusiness(IAppointmentRepository appointmentRepository) =>
        _appointmentRepository = appointmentRepository;

    public Task<bool> AddAppointment(AppointmentDetails appointment)
    {
        ArgumentNullException.ThrowIfNull(appointment);
        ValidateAppointment(appointment);
        return _appointmentRepository.AddAppointment(appointment);
    }

    public Task<IReadOnlyList<AppointmentDetails>> GetAllAppointments() =>
        _appointmentRepository.GetAllAppointments();

    public Task<IReadOnlyList<AppointmentDetails>> GetAppointmentsByPatient(int patientId)
    {
        if (patientId <= 0) throw new ArgumentOutOfRangeException(nameof(patientId));
        return _appointmentRepository.GetAppointmentsByPatient(patientId);
    }

    public async Task<IReadOnlyList<AppointmentDetails>> GetAppointmentsByDate(DateTime date) =>
        (await _appointmentRepository.GetAllAppointments())
            .Where(appointment => appointment.AppointmentDate.Date == date.Date)
            .ToList();

    public Task<bool> UpdateStatus(int appointmentId, string status)
    {
        if (appointmentId <= 0) throw new ArgumentOutOfRangeException(nameof(appointmentId));
        if (string.IsNullOrWhiteSpace(status)) throw new ArgumentException("Status is required.", nameof(status));
        return _appointmentRepository.UpdateStatus(appointmentId, status.Trim());
    }

    public Task<int> GetTodayAppointmentCount(DateTime date) => _appointmentRepository.GetTodayAppointmentCount(date);

    private static void ValidateAppointment(AppointmentDetails appointment)
    {
        if (appointment.PatientId <= 0) throw new ArgumentException("PatientId is required.");
        if (appointment.DoctorId <= 0) throw new ArgumentException("DoctorId is required.");
        if (appointment.AppointmentDate == default) throw new ArgumentException("Appointment date is required.");
        if (string.IsNullOrWhiteSpace(appointment.Status)) throw new ArgumentException("Status is required.");
    }
}
