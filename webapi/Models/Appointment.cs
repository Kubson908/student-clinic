using Przychodnia.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Przychodnia.Webapi.Models
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
        public string? Diagnosis { get; set; }
        public string? Recommendations { get; set; }
        [Required]
        public bool Finished { get; set; } = false;

        //Navigation Properties
        [Required]
        [ForeignKey("Patient")]
        public string PatientId { get; set; } = string.Empty;
        public virtual Patient? Patient { get; set; }

        [ForeignKey("Doctor")]
        public string? DoctorId { get; set; }
        public Employee? Doctor { get; set; }

        [ForeignKey("ControlAppointment")]
        public int? AppointmentId { get; set; }
        public Appointment? ControlAppointment { get; set; }
    }
}
