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
        public int PersonalId { get; set; }

        [Required]
        public string NumberHash { get; set; } = "";

        [Required]
        public string Email { get; set; } = "";

        public string Token { get; set; } = "";

        public DateTime RegisterDate { get; set; }

        public DateTime UpdateDate { get; set; }

        // relations
        public ICollection<Property> Properties { get; set; } = null!;

        public Personal Personal { get; set; } = null!;

        public string GetProfileLink()
        {
            return $"/user/{Id}";
        }
    }
}
