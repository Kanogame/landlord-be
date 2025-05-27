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

    public class PropertyGetSearchRespElDTO : Property
    {
        public PropertyGetSearchRespElDTO(Property prop, string username, string profileLink)
        {
            Id = prop.Id;
            OwnerId = prop.OwnerId;
            OfferTypeId = prop.OfferTypeId;
            PropertyTypeId = prop.PropertyTypeId;
            Status = prop.Status;
            Name = prop.Name;
            Desc = prop.Desc;
            AddressId = prop.AddressId;
            Area = prop.Area;
            Price = prop.Price;
            Currency = prop.Currency;
            Raiting = prop.Raiting;
            Period = prop.Period;
            User = prop.User;
            ImageLinks = prop.ImageLinks;
            Address = prop.Address;
            PropertyAttributes = prop.PropertyAttributes;

            PropertyLink = prop.GetPropertyLink();
            Username = username;
            ProfileLink = profileLink;
        }

        public string PropertyLink { get; set; }
        public string Username { get; set; }
        public string ProfileLink { get; set; }
    }
}
