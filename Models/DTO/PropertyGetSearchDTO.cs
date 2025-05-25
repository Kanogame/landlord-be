using System.ComponentModel.DataAnnotations;

namespace landlord_be.Models.DTO
{
    public class PropertyGetSearchDTO
    {
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [Range(10, 100)]
        public int PageSize { get; set; } = 10;

    }

    public class PropertyFiltersDTO
    {
        public string
    }
}