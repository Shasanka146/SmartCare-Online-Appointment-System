using System.ComponentModel.DataAnnotations;

namespace SmartCare.Shared.AppointmentData;

public class AppointmentDetails
{
    public int AppointmentId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "A patient must be selected.")]
    public int PatientId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "A doctor must be selected.")]
    public int DoctorId { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime AppointmentDate { get; set; }

    [Required, StringLength(30)]
    public string Status { get; set; } = "Scheduled";
}
