using System.ComponentModel.DataAnnotations;

namespace landlord_be.Models
{
    public class Bookmark
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int PropertyId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public User User { get; set; } = null!;
        public Property Property { get; set; } = null!;
    }
}
