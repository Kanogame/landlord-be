
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace landlord_be.Models
{
    public enum OfferType
    {
        Rent, Sell
    }

    public enum PropertyType
    {
        Flat, Detached, Commercial
    }

    public class Property
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Owner
        [Required]
        [ForeignKey("User")]
        public int OwnerId { get; set; }

        [Required]
        public OfferType OfferTypeId { get; set; }

        [Required]
        public PropertyType PropertyTypeId { get; set; }

        // Property name
        [Required]
        [StringLength(255)]
        public string Name { get; set; } = "";

        public string Desc { get; set; } = "";

        [Required]
        public int AddressId { get; set; }

        [Required]
        public int Area { get; set; }

        public int ImageLinkId { get; set; }

        // relations
        public User? User { get; set; }
        public IEnumerable<ImageLink>? ImageLinks { get; set; }
        public Address? Address { get; set; }
    }
}