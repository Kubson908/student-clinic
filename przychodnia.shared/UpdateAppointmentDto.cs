using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Przychodnia.Shared
{
    public class UpdateAppointmentDto
    {
        public DateTime? Date { get; set; }
        public Specialization? Specialization { get; set; }
        public string? Symptoms { get; set; }
        public string? Medicines { get; set; }
        public string? Recommendations { get; set; }
        public bool? Finished { get; set; }

        public string? DoctorId { get; set; }
        public int? AppointmentId { get; set; }
    }
}
