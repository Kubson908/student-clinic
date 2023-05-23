using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Przychodnia.Shared
{
    public class GetAvailableDoctorsDto
    {
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public Specialization Specialization { get; set; }
    }
}
