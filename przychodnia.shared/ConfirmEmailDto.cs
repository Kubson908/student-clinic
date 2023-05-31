using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Przychodnia.Shared
{
    public class ConfirmEmailDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        public string? Token { get; set; }
    }
}
