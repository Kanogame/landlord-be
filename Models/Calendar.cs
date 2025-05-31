using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace landlord_be.Models
{
    public enum CalendarState
    {
        Available,
        Rented,
        Sold,
        Maintenance,
        Blocked,
        Reserved,
    }

    public class CalendarPeriod
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Property")]
        public int PropertyId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public CalendarState State { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = "";

        [StringLength(5000)] // Increased from 1000 to 5000 for detailed descriptions
        public string Description { get; set; } = "";

        // Optional user attachment for renting/selling periods
        [ForeignKey("User")]
        public int? AttachedUserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Property? Property { get; set; }
        public User? AttachedUser { get; set; }
    }
}
