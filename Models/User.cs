using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace landlord_be.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string OwnerId { get; set; } = "";

        [Required]
        public string PasswordHash { get; set; } = "";

        [Required]
        public string Token { get; set; } = "";

        public DateTime RegisterDate { get; set; }

        public DateTime UpdateDate { get; set; }

        // relations
        public ICollection<Property>? Properties { get; set; }
    }
}