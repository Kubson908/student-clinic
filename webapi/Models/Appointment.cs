using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public Specialization Specialization { get; set; }
        public string? Symptoms { get; set; }
        public string? Medicines { get; set; }
        public string? Recommendations { get; set; }

        //Navigation Properties
        [Required]
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }

        [ForeignKey("Doctor")]
        public int? DoctorId { get; set; }
        public Worker? Doctor { get; set; }

        [ForeignKey("ControlAppointment")]
        public int? AppointmentId { get; set; }
        public Appointment? ControlAppointment { get; set; }
    }
}
