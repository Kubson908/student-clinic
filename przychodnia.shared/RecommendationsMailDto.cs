using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Przychodnia.Shared
{
    public class RecommendationsMailDto
    {
        public string Email { get; set; } = string.Empty;
        public string? Diagnosis { get; set; } = string.Empty;
        public string? Medicines { get; set; } = string.Empty;
        public string? Recommendations { get; set; } = string.Empty;
        public string AppointmentDate { get; set; } = string.Empty;
        public string? ControlAppointmentDate { get; set; } = string.Empty;

    }
}
