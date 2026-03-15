using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareBusiness.Entities
{
    [Table("Doctors")]
    public class Doctor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string DoctorName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Specialty { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string LicenseNumber { get; set; } = string.Empty;

        public int MaxPatients { get; set; } = 0;

        public bool Active { get; set; } = true;

        public ICollection<Appointment>? Appointments { get; set; }
    }
}
