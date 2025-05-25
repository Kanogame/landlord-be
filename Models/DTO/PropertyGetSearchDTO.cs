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

        public OfferType? OfferType { get; set; }

    }
    public class PropertyGetSearchRespDTO
    {
        public int Count { get; set; }

        public IEnumerable<Property>? Properties { get; set; }

    }

}