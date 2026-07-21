namespace SmartCare.Models;

public class DashboardStats
{
    public int TotalClinics { get; init; }
    public int TotalDoctors { get; init; }
    public int TotalPatients { get; init; }
    public int TodayAppointments { get; init; }
    public int AvailableDoctors { get; init; }
    public int BusyDoctors { get; init; }
}
