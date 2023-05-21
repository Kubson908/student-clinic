﻿using System.ComponentModel.DataAnnotations;

namespace Przychodnia.Shared
{
    public class RegisterDto
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string DateOfBirth { get; set; } = string.Empty;

        [Required]
        [StringLength(11)]
        public string Pesel { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [StringLength(12, MinimumLength = 9)]
        public string PhoneNumber { get; set; } = string.Empty;

    }

}
