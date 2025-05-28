using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace landlord_be.Models
{
    public class VerificationPending
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string NumberHash { get; set; } = "";

        [Required]
        public int VerificationCode { get; set; }

        [Required]
        public bool Verified { get; set; }
    }
}
