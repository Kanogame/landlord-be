using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace landlord_be.Models
{
    public class ChatMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ChatId { get; set; }

        [Required]
        public int SenderId { get; set; }

        [Required]
        [StringLength(1000)]
        public string Content { get; set; } = "";

        [Required]
        public DateTime SentDate { get; set; }

        [Required]
        public bool IsRead { get; set; } = false;

        // Navigation properties
        public Chat Chat { get; set; } = null!;
        public User Sender { get; set; } = null!;
    }
}
