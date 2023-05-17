using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Przychodnia.Webapi.Models
{
    public class Worker : IdentityUser
    { 

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string DateOfBirth { get; set; }

        [Required]
        public Specialization Specialization { get; set; }
        
        [Required]
        [StringLength(11)]
        public string Pesel { get; set; }

    }

    public enum Specialization
    {
        Internista, Pulmonolog, Okulista, Gastrolog
    }
}
