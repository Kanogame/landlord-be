using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace landlord_be.Models
{
    public enum OfferType
    {
        Rent,
        Sell,
    }

    public enum PropertyType
    {
        Flat,
        Detached,
        Commercial,
    }

    public enum RentPeriod
    {
        Month,
        Week,
        Day,
    }

    public enum PropertyStatus
    {
        Draft,
        Active,
        Rented,
        RentEnding,
        Sold,
        Hidden,
        UnderMaintenance,
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

        // status
        public PropertyStatus Status { get; set; }

        [NotMapped]
        public bool IsDisplayed
        {
            get
            {
                if (Status == PropertyStatus.Active || Status == PropertyStatus.RentEnding)
                {
                    return true;
                }
                return false;
            }
        }

        // Generic values
        [Required]
        [StringLength(255)]
        public string Name { get; set; } = "";

        public string Desc { get; set; } = "";

        [Required]
        public int AddressId { get; set; }

        [Required]
        public int Area { get; set; }

        // Finance
        public int Price { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Currency { get; set; }

        // Rent only
        public float Raiting { get; set; }

        public RentPeriod Period { get; set; }

        // relations
        public User? User { get; set; }
        public IEnumerable<ImageLink>? ImageLinks { get; set; }
        public Address? Address { get; set; }
        public ICollection<PropertyAttribute>? PropertyAttributes { get; set; }

        public string GetPropertyLink()
        {
            return $"/property/{Id}";
        }
    }
}
