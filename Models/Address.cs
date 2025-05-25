using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using landlord_be.Models;

namespace landlord_be
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string City { get; set; } = "";

        [Required]
        [StringLength(255)]
        public string District { get; set; } = "";

        [Required]
        [StringLength(255)]
        public string Street { get; set; } = "";

        [Required]
        public int Story { get; set; }

        [NotMapped]
        public string DisplayAddress { 
            get {
            return $"{Street} г.{City}";
            }
        }

        // relations
        public Property? Property { get; set; }
    }
}