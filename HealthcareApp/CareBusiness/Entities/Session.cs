using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareBusiness.Entities
{
    [Table("Sessions")]
    public class Session
    {
        [Key]
        [StringLength(50)]
        public string SessionID { get; set; } = string.Empty;

        [Required]
        public int UserID { get; set; }

        [Required]
        [StringLength(20)]
        public string Role { get; set; } = string.Empty;

        [Required]
        public DateTime ExpiresAt { get; set; }

        [ForeignKey("UserID")]
        public User? User { get; set; }
    }
}
