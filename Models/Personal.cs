using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace landlord_be.Models
{
    public class Personal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string FirstName { get; set; } = "";

        [Required]
        [StringLength(255)]
        public string LastName { get; set; } = "";

        [Required]
        [StringLength(255)]
        public string Patronym { get; set; } = "";

        [NotMapped]
        public string FullName
        {
            get { return $"{FirstName} {LastName} {Patronym}"; }
        }

        // relations
        public User User { get; set; } = null!;
    }
}
