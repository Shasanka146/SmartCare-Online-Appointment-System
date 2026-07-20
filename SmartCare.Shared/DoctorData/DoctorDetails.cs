using System.ComponentModel.DataAnnotations;

namespace SmartCare.Shared.DoctorData;

public class DoctorDetails
{
    [Key]
    public int DoctorId { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string Status { get; set; } = "Available";
}
