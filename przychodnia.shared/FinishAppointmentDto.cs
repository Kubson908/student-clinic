using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Przychodnia.Shared
{
    public class FinishAppointmentDto
    {
        public bool Finished { get; set; }
        public string? Diagnosis { get; set; }
        public string? Recommendations { get; set; }
        public string? Meds { get; set; }
        public DateTime? ControlDate { get; set; }
    }
}
