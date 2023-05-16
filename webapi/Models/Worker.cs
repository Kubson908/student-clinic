using System.ComponentModel.DataAnnotations;

namespace webapi.Models
{
    public class Worker
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string DateOfBirth { get; set; }
        [Required]
        public Specialization Specialization { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [StringLength(11)]
        public string Pesel { get; set; }

        [Required]
        [MaxLength(12)]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }
    }

    public enum Specialization
    {
        Internista, Pulmonolog, Okulista, Gastrolog
    }
}
