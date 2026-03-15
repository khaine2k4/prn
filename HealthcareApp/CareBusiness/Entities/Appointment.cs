using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareBusiness.Entities
{
    [Table("Appointments")]
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public int PatientID { get; set; }

        [Required]
        public int DoctorID { get; set; }

        public DateTime BookingDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime AppointmentDate { get; set; }

        public bool IsCancelled { get; set; } = false;

        [ForeignKey("PatientID")]
        public User? Patient { get; set; }

        [ForeignKey("DoctorID")]
        public Doctor? Doctor { get; set; }
    }
}
