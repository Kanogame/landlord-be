using System.ComponentModel.DataAnnotations;

namespace landlord_be.Models.DTO
{
    public class CreatePropertyDTO
    {
        [Required]
        public OfferType OfferTypeId { get; set; }

        [Required]
        public PropertyType PropertyTypeId { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = "";

        public string? Desc { get; set; }

        [Required]
        public int AddressId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Area { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        public int Currency { get; set; } = 125; // Default currency

        public RentPeriod Period { get; set; } = RentPeriod.Month;

        public List<CreatePropertyAttributeDTO>? PropertyAttributes { get; set; }
    }

    public class UpdatePropertyDTO
    {
        public OfferType? OfferTypeId { get; set; }
        public PropertyType? PropertyTypeId { get; set; }

        [StringLength(255)]
        public string? Name { get; set; }

        public string? Desc { get; set; }
        public int? AddressId { get; set; }

        [Range(1, int.MaxValue)]
        public int? Area { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal? Price { get; set; }

        public int? Currency { get; set; }
        public RentPeriod? Period { get; set; }
        public PropertyStatus? Status { get; set; }

        public List<CreatePropertyAttributeDTO>? PropertyAttributes { get; set; }
    }

    public class CreatePropertyAttributeDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = "";

        [StringLength(500)]
        public string Value { get; set; } = "";

        [Required]
        public PropertyAttributeType AttributeType { get; set; }

        public bool IsSearchable { get; set; } = true;
    }
}
