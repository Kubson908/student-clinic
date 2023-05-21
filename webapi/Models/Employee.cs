using Microsoft.AspNetCore.Identity;
using Przychodnia.Shared;
using System.ComponentModel.DataAnnotations;

namespace Przychodnia.Webapi.Models
{
    public class Employee : IdentityUser
    {

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string DateOfBirth { get; set; } = string.Empty;

        [Required]
        public Specialization Specialization { get; set; }

        [Required]
        [StringLength(11)]
        public string Pesel { get; set; } = string.Empty;

    }
}
