using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SmartCare.Shared.PatientData
{
    public class PatientDetails
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        public int PatientId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        [StringLength(15)]
        public string Contactno { get; set; }

        [Required,StringLength(100)]
        public string Address { get; set; }
    }

}
