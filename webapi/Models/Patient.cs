using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Przychodnia.Webapi.Models
{
    public class Patient : IdentityUser
    { 
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string DateOfBirth { get; set; }
    
        [Required]
        [StringLength(11)]
        public string Pesel { get; set; }

        //Navigation Properties
        public virtual List<Appointment> Appointments { get; set; } = new List<Appointment>();

    }
}
