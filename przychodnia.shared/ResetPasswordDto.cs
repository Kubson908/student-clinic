using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Przychodnia.Shared
{
    public class ResetPasswordDto
    {
        [Required]
        public string Id { get; set; } = "";
        [Required]
        public string Token { get; set; } = "";
        [Required]
        [MinLength(8)]
        public string Password { get; set; } = "";
    }
}
