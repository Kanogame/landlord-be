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
        public string Region { get; set; } = "";

        [Required]
        [StringLength(255)]
        public string City { get; set; } = "";

        [Required]
        [StringLength(255)]
        public string Street { get; set; } = "";

        [Required]
        [StringLength(255)]
        public string House { get; set; } = "";

        [Required]
        public int Floor { get; set; }

        [NotMapped]
        public string DisplayAddress
        {
            get { return $"г.{City}, {Street} {House}"; }
        }

        // relations
        public Property? Property { get; set; }
    }
}
