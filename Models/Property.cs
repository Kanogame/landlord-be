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

        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

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

        [Required]
        public int Rooms { get; set; }

        [Required]
        public bool Services { get; set; } //ЖКХ

        [Required]
        public bool Parking { get; set; }

        // Finance
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int Currency { get; set; }

        // Rent only
        public float Rating { get; set; }

        public RentPeriod Period { get; set; }

        // relations
        public User User { get; set; } = null!;
        public IEnumerable<ImageLink> ImageLinks { get; set; } = null!;
        public Address Address { get; set; } = null!;
        public ICollection<PropertyAttribute> PropertyAttributes { get; set; } = null!;

        public string GetPropertyLink()
        {
            return $"/property/{Id}";
        }
    }

    public class DTOProperty
    {
        public DTOProperty(Property prop, string? username, string? profileLink, bool? isBookmarked)
        {
            Id = prop.Id;
            OwnerId = prop.OwnerId;
            OfferTypeId = prop.OfferTypeId;
            PropertyTypeId = prop.PropertyTypeId;
            Name = prop.Name;
            Desc = prop.Desc;
            Address = prop.Address;
            Area = prop.Area;
            Rooms = prop.Rooms;
            Services = prop.Services;
            Parking = prop.Parking;
            Price = new Money(prop.Price, prop.Currency);
            ImageLinks = prop.ImageLinks;
            Rating = prop.Rating;
            Period = prop.Period;
            PropertyAttributes = prop.PropertyAttributes;
            CreatedAt = prop.CreatedAt;
            UpdatedAt = prop.UpdatedAt;

            PropertyLink = prop.GetPropertyLink();
            Username = username ?? "";
            ProfileLink = profileLink ?? "not_found";
            IsBookmarked = isBookmarked;
        }

        public int Id { get; set; }

        public int OwnerId { get; set; }
        public OfferType OfferTypeId { get; set; }
        public PropertyType PropertyTypeId { get; set; }

        public string Name { get; set; } = "";

        public string Desc { get; set; } = "";
        public int Area { get; set; }

        public int Rooms { get; set; }

        public bool Services { get; set; }

        public bool Parking { get; set; }

        // Finance
        public Money Price { get; set; }

        // Rent only
        public float Rating { get; set; }

        public RentPeriod Period { get; set; }

        // Timestamps
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Address Address { get; set; } = null!;

        public IEnumerable<ImageLink> ImageLinks { get; set; } = null!;
        public ICollection<PropertyAttribute> PropertyAttributes { get; set; } = null!;

        public string PropertyLink { get; set; }
        public string Username { get; set; }
        public string ProfileLink { get; set; }
        public bool? IsBookmarked { get; set; }
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
        public DTOPropertyWithType(
            Property property,
            string? username,
            string? profileLink,
            bool? isBookmarked
        )
        {
            Type = property.OfferTypeId;
            Property = new DTOProperty(property, username, profileLink, isBookmarked);
        }

        public OfferType Type { get; set; }
        public DTOProperty Property { get; set; }
    }
}
