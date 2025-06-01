using landlord_be.Models;
using landlord_be.Models.DTO;

public class GetOwnPropertiesReqDTO
{
    public PropertySortBy SortBy { get; set; } = PropertySortBy.CreatedDesc;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public PropertyStatus? Status { get; set; }
}

public class GetOwnPropertiesRespDTO
{
    public int Count { get; set; }
    public List<DTOPropertyWithTypeAndStatus> Properties { get; set; } = new();
}

public class DTOPropertyWithTypeAndStatus
{
    public DTOPropertyWithTypeAndStatus(
        Property property,
        string? username,
        string? profileLink,
        bool? isBookmarked,
        PropertyStatus status
    )
    {
        Type = property.OfferTypeId;
        Property = new DTOProperty(property, username, profileLink, isBookmarked);
        Status = status;
    }

    public OfferType Type { get; set; }
    public DTOProperty Property { get; set; }
    public PropertyStatus Status { get; set; }
}
