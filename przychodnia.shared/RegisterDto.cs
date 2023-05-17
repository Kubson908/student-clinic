using System.ComponentModel.DataAnnotations;

namespace Przychodnia.Shared
{
    public class RegisterDto
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        public string DateOfBirth { get; set; }

        [Required]
        [StringLength(11)]
        public string Pesel { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 9)]
        public string PhoneNumber { get; set; }
    }
}
