using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Przychodnia.Shared
{
    public class UpdatePatientDto
    {
        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        public string? DateOfBirth { get; set; }

        [StringLength(11)]
        public string? Pesel { get; set; }

        [StringLength(50)]
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(50, MinimumLength = 8)]
        public string? Password { get; set; }

        [StringLength(50, MinimumLength = 8)]
        public string? ConfirmPassword { get; set; }

        [StringLength(12, MinimumLength = 9)]
        public string? PhoneNumber { get; set; }
        
        public string? Allergies { get; set; }
        public string? Medicines { get; set; }
        public bool? Verified { get; set; }
    }
}
