using System.ComponentModel.DataAnnotations;

namespace SmartCare.Shared.PatientData;

public class PatientDetails
{
    [Key]
    public int PatientId { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public int UserId { get; set; }

    [Required, Range(0, 150)]
    public int Age { get; set; }

    [Required, StringLength(20)]
    public string Gender { get; set; } = string.Empty;

    [Required, StringLength(15)]
    public string Contactno { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Address { get; set; } = string.Empty;
}
