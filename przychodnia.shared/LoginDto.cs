using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Przychodnia.Shared
{
    public class LoginDto
    {
        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
