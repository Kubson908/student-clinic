using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Przychodnia.Webapi.Models
{
    public class Patient : IdentityUser
    { 
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string DateOfBirth { get; set; } = string.Empty;

        [Required]
        [StringLength(11)]
        public string Pesel { get; set; } = string.Empty;

        //Navigation Properties
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();

    }
}
