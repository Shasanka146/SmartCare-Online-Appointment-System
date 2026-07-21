using System;

namespace SmartCare.Shared.ClinicData
{
    public class ClinicDetails
    {
        public int ClinicId { get; set; }
        public string ClinicName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
