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
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int Currency { get; set; }

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

    public class DTOProperty
    {
        public DTOProperty(Property prop, string? username, string? profileLink)
        {
            Id = prop.Id;
            OwnerId = prop.OwnerId;
            OfferTypeId = prop.OfferTypeId;
            PropertyTypeId = prop.PropertyTypeId;
            Name = prop.Name;
            Desc = prop.Desc;
            Address = prop.Address;
            Area = prop.Area;
            Price = new Money(prop.Price, prop.Currency);
            ImageLinks = prop.ImageLinks;
            Raiting = prop.Raiting;
            Period = prop.Period;
            PropertyAttributes = prop.PropertyAttributes;

            PropertyLink = prop.GetPropertyLink();
            Username = username != null ? username : "";
            ProfileLink = profileLink != null ? profileLink : "not_found";
        }

        public int Id { get; set; }

        public int OwnerId { get; set; }
        public OfferType OfferTypeId { get; set; }
        public PropertyType PropertyTypeId { get; set; }

        public string Name { get; set; } = "";

        public string Desc { get; set; } = "";
        public int Area { get; set; }

        // Finance
        public Money Price { get; set; }

        // Rent only
        public float Raiting { get; set; }

        public RentPeriod Period { get; set; }

        public Address? Address { get; set; }

        public IEnumerable<ImageLink>? ImageLinks { get; set; }
        public ICollection<PropertyAttribute>? PropertyAttributes { get; set; }

        public string PropertyLink { get; set; }
        public string Username { get; set; }
        public string ProfileLink { get; set; }
    }

    public class Money
    {
        public Money(decimal amount, int currency)
        {
            Amount = amount.ToString();
            Currency = currency;
        }

        public string Amount { get; set; }
        public int Currency { get; set; }

        public string CurrencySymbol { get; set; } = "P";
    }

    public class DTOPropertyWithType
    {
        public DTOPropertyWithType(Property property, string? username, string? profileLink)
        {
            Type = property.OfferTypeId;
            Property = new DTOProperty(property, username, profileLink);
        }

        public OfferType Type { get; set; }
        public DTOProperty Property { get; set; }
    }
}
