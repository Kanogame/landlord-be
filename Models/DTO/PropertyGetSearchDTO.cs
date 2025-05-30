using System.ComponentModel.DataAnnotations;

namespace landlord_be.Models.DTO
{
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

        public decimal? PriceFrom { get; set; }

        public decimal? PriceTo { get; set; }

        public int? FloorFrom { get; set; }

        public int? FloorTo { get; set; }

        public int? Area { get; set; }

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
