using System.ComponentModel.DataAnnotations;

namespace landlord_be.Models.DTO
{
    public enum PropertySortBy
    {
        PriceAsc,
        PriceDesc,
        AreaAsc,
        AreaDesc,
        CreatedAsc,
        CreatedDesc,
    }

    public class PropertyGetSearchReqDTO
    {
        [Range(1, int.MaxValue)]
        [Required(ErrorMessage = "PageNumber is required")]
        public int PageNumber { get; set; } = 1;

        [Range(10, 100)]
        [Required(ErrorMessage = "PageSize is required")]
        public int PageSize { get; set; } = 10;

        [Required(ErrorMessage = "OfferType is required")]
        public int OfferType { get; set; }

        public int? PropertyType { get; set; }

        public string? City { get; set; }

        public string? District { get; set; }

        public string? Region { get; set; }

        public string? Street { get; set; }

        public decimal? PriceFrom { get; set; }

        public decimal? PriceTo { get; set; }

        public int? FloorFrom { get; set; }

        public int? FloorTo { get; set; }

        public int? AreaFrom { get; set; }

        public int? RoomsFrom { get; set; }

        public bool? Services { get; set; }

        public bool? Parking { get; set; }

        public RentPeriod? Period { get; set; }

        public PropertySortBy? SortBy { get; set; } = PropertySortBy.CreatedDesc;

        public IEnumerable<PropertyAttributeDTO> Attributes { get; set; } =
            new List<PropertyAttributeDTO>();
    }

    public class PropertyAttributeDTO
    {
        public string Name { get; set; } = "";
        public string Value { get; set; } = "";
    }

    public class PropertyGetSearchRespDTO
    {
        public int Count { get; set; }

        public IEnumerable<DTOPropertyWithType>? Properties { get; set; }
    }
}
