using System.Security.Claims;
using landlord_be.Data;
using landlord_be.Models;
using landlord_be.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace landlord_be.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PropertyController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PropertyController(ApplicationDbContext context)
    {
        _context = context;
    }

    private int? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out var userId) ? userId : null;
    }

    private async Task<bool> IsPropertyBookmarked(int propertyId, int? userId)
    {
        if (userId == null)
            return false;

        return await _context.Bookmarks.AnyAsync(b =>
            b.UserId == userId && b.PropertyId == propertyId
        );
    }

    private async Task<Dictionary<int, bool>> GetBookmarkStatusForProperties(
        List<int> propertyIds,
        int? userId
    )
    {
        if (userId == null)
            return propertyIds.ToDictionary(id => id, id => false);

        var bookmarkedPropertyIds = await _context
            .Bookmarks.Where(b => b.UserId == userId && propertyIds.Contains(b.PropertyId))
            .Select(b => b.PropertyId)
            .ToListAsync();

        return propertyIds.ToDictionary(id => id, id => bookmarkedPropertyIds.Contains(id));
    }

    [HttpGet("get_search_attributes")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<PropertySearchAttributeDTO>>> GetSearchAttributes()
    {
        var searchableAttributes = await _context
            .Attributes.Where(a => a.IsSearchable)
            .GroupBy(a => new { a.Name, a.AttributeType })
            .Select(g => new PropertySearchAttributeDTO
            {
                AttributeName = g.Key.Name,
                AttributeType = g.Key.AttributeType,
                PossibleValues =
                    g.Key.AttributeType == PropertyAttributeType.Text
                        ? g.Select(a => a.Value).Distinct().ToList()
                        : new List<string>(),
            })
            .OrderBy(a => a.AttributeName)
            .ToListAsync();

        return Ok(searchableAttributes);
    }

    [HttpPost("get_properties_by_user_id")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<PropertyGetSearchRespDTO>>> GetPropertiesByUserId(
        PropertyGetByUserDTO req
    )
    {
        bool userExists = await _context.Users.AnyAsync(u => u.Id == req.UserId);
        if (!userExists)
        {
            return BadRequest(new BadRequestMessage($"No user with id {req.UserId}"));
        }

        IQueryable<Property> query = _context.Properties.Where(p =>
            p.Status == PropertyStatus.Active || p.Status == PropertyStatus.RentEnding
        );

        query = query.Where(p => p.OwnerId == req.UserId);

        var totalCount = await query.CountAsync();
        var properties = await _context
            .Properties.Include(p => p.User)
            .ThenInclude(u => u.Personal)
            .Include(p => p.Address)
            .Include(p => p.ImageLinks)
            .Include(p => p.PropertyAttributes)
            .AsSplitQuery()
            .AsNoTracking()
            .ToListAsync();

        // Get current user ID for bookmark checking
        var currentUserId = GetCurrentUserId();

        // Get bookmark status for all properties if user is authenticated
        var propertyIds = properties.Select(p => p.Id).ToList();
        var bookmarkStatuses = await GetBookmarkStatusForProperties(propertyIds, currentUserId);

        var items = properties
            .Select(p => new DTOPropertyWithType(
                p,
                p.User?.Personal?.FirstName ?? null,
                p.User?.GetProfileLink() ?? null,
                currentUserId.HasValue ? bookmarkStatuses[p.Id] : null
            ))
            .ToList();

        return Ok(new PropertyGetSearchRespDTO { Count = totalCount, Properties = items });
    }

    [HttpPost("get_property_by_id")]
    [AllowAnonymous]
    public async Task<ActionResult<DTOPropertyWithType>> GetPropertyById(PropertyGetByIdDTO req)
    {
        bool propertyExists = await _context.Properties.AnyAsync(p => p.Id == req.PropertyId);
        if (!propertyExists)
        {
            return BadRequest(new BadRequestMessage($"No property with id {req.PropertyId}"));
        }

        var property = await _context
            .Properties.Include(p => p.User)
            .ThenInclude(u => u.Personal)
            .Include(p => p.Address)
            .Include(p => p.ImageLinks)
            .Include(p => p.PropertyAttributes)
            .AsSplitQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == req.PropertyId);

        var currentUserId = GetCurrentUserId();
        bool? isBookmarked = null;

        if (currentUserId.HasValue)
        {
            isBookmarked = await IsPropertyBookmarked(req.PropertyId, currentUserId);
        }

        var dtoProperty = new DTOPropertyWithType(
            property,
            property.User?.Personal?.FirstName,
            property.User?.GetProfileLink(),
            isBookmarked
        );

        return Ok(dtoProperty);
    }

    [HttpPost("get_properties_search")]
    [AllowAnonymous]
    public async Task<ActionResult<PropertyGetSearchRespDTO>> GetPropertiesSearch(
        PropertyGetSearchReqDTO req
    )
    {
        // Start with base query including necessary joins for filtering
        IQueryable<Property> query = _context
            .Properties.Include(p => p.Address)
            .Where(p => p.Status == PropertyStatus.Active || p.Status == PropertyStatus.RentEnding);

        // Apply filters
        query = query.Where(p => p.OfferTypeId == (OfferType)req.OfferType);

        if (req.PropertyType.HasValue)
        {
            query = query.Where(p => p.PropertyTypeId == (PropertyType)req.PropertyType.Value);
        }

        if (!string.IsNullOrWhiteSpace(req.City))
        {
            query = query.Where(p => p.Address!.City.ToLower().Contains(req.City.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(req.District))
        {
            query = query.Where(p =>
                p.Address!.District.ToLower().Contains(req.District.ToLower())
            );
        }

        if (req.PriceFrom.HasValue)
        {
            query = query.Where(p => p.Price >= req.PriceFrom.Value);
        }

        if (req.PriceTo.HasValue)
        {
            query = query.Where(p => p.Price <= req.PriceTo.Value);
        }

        if (req.FloorFrom.HasValue)
        {
            query = query.Where(p => p.Address!.Story >= req.FloorFrom.Value);
        }

        if (req.FloorTo.HasValue)
        {
            query = query.Where(p => p.Address!.Story <= req.FloorTo.Value);
        }

        if (req.Area.HasValue)
        {
            query = query.Where(p => p.Area >= req.Area.Value);
        }

        // Apply attribute filters if any
        if (req.Attributes.Any())
        {
            foreach (var attribute in req.Attributes)
            {
                var attributeName = attribute.Name;
                var attributeValue = attribute.Value;

                // For each attribute filter, we need to check if the property has this attribute with the specified value
                query = query.Where(p =>
                    p.PropertyAttributes!.Any(pa =>
                        pa.Name == attribute.Name
                        && pa.Value.ToLower().Contains(attributeValue.ToLower())
                    )
                );
            }
        }

        // Get total count before pagination (but after all filters)
        var totalCount = await query.CountAsync();

        // Apply pagination and get final results with all necessary data
        var properties = await query
            .OrderBy(p => p.Id) // Add consistent ordering for pagination
            .Skip((req.PageNumber - 1) * req.PageSize)
            .Take(req.PageSize)
            .Include(p => p.User)
            .ThenInclude(u => u.Personal)
            .Include(p => p.Address)
            .Include(p => p.ImageLinks)
            .Include(p => p.PropertyAttributes)
            .AsSplitQuery()
            .AsNoTracking()
            .ToListAsync();

        // Get current user ID for bookmark checking
        var currentUserId = GetCurrentUserId();

        // Get bookmark status for all properties if user is authenticated
        var propertyIds = properties.Select(p => p.Id).ToList();
        var bookmarkStatuses = await GetBookmarkStatusForProperties(propertyIds, currentUserId);

        var items = properties
            .Select(p => new DTOPropertyWithType(
                p,
                p.User != null && p.User.Personal != null ? p.User.Personal.FirstName : null,
                p.User != null ? p.User.GetProfileLink() : null,
                currentUserId.HasValue ? bookmarkStatuses[p.Id] : null
            ))
            .ToList();

        return Ok(new PropertyGetSearchRespDTO { Count = totalCount, Properties = items });
    }

    [HttpPost("create")]
    public async Task<ActionResult<DTOPropertyWithType>> CreateProperty(CreatePropertyDTO req)
    {
        var currentUserId = GetCurrentUserId();
        if (!currentUserId.HasValue)
        {
            return Unauthorized();
        }

        // Validate address exists
        var addressExists = await _context.Addresses.AnyAsync(a => a.Id == req.AddressId);
        if (!addressExists)
        {
            return BadRequest(new BadRequestMessage($"Address with id {req.AddressId} not found"));
        }

        var property = new Property
        {
            OwnerId = currentUserId.Value,
            OfferTypeId = req.OfferTypeId,
            PropertyTypeId = req.PropertyTypeId,
            Name = req.Name,
            Desc = req.Desc ?? "",
            AddressId = req.AddressId,
            Area = req.Area,
            Price = req.Price,
            Currency = req.Currency,
            Period = req.Period,
            Status = PropertyStatus.Draft,
        };

        _context.Properties.Add(property);
        await _context.SaveChangesAsync();

        // Add property attributes if provided
        if (req.PropertyAttributes != null && req.PropertyAttributes.Any())
        {
            var attributes = req
                .PropertyAttributes.Select(attr => new PropertyAttribute
                {
                    PropertyId = property.Id,
                    Name = attr.Name,
                    Value = attr.Value,
                    AttributeType = attr.AttributeType,
                    IsSearchable = attr.IsSearchable,
                })
                .ToList();

            _context.Attributes.AddRange(attributes);
            await _context.SaveChangesAsync();
        }

        // Reload property with all related data
        var createdProperty = await _context
            .Properties.Include(p => p.User)
            .ThenInclude(u => u.Personal)
            .Include(p => p.Address)
            .Include(p => p.ImageLinks)
            .Include(p => p.PropertyAttributes)
            .AsSplitQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == property.Id);

        var dtoProperty = new DTOPropertyWithType(
            createdProperty,
            createdProperty.User?.Personal?.FirstName,
            createdProperty.User?.GetProfileLink(),
            false // Owner's own property, not bookmarked
        );

        return Ok(dtoProperty);
    }

    [HttpPut("update/{id}")]
    public async Task<ActionResult<DTOPropertyWithType>> UpdateProperty(
        int id,
        UpdatePropertyDTO req
    )
    {
        var currentUserId = GetCurrentUserId();
        if (!currentUserId.HasValue)
        {
            return Unauthorized();
        }

        var property = await _context
            .Properties.Include(p => p.PropertyAttributes)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (property == null)
        {
            return NotFound(new BadRequestMessage($"Property with id {id} not found"));
        }

        // Check if user owns the property
        if (property.OwnerId != currentUserId.Value)
        {
            return Forbid("You can only update your own properties");
        }

        // Validate address exists if being updated
        if (req.AddressId.HasValue)
        {
            var addressExists = await _context.Addresses.AnyAsync(a => a.Id == req.AddressId.Value);
            if (!addressExists)
            {
                return BadRequest(
                    new BadRequestMessage($"Address with id {req.AddressId} not found")
                );
            }
            property.AddressId = req.AddressId.Value;
        }

        // Update property fields
        if (req.OfferTypeId.HasValue)
            property.OfferTypeId = req.OfferTypeId.Value;
        if (req.PropertyTypeId.HasValue)
            property.PropertyTypeId = req.PropertyTypeId.Value;
        if (!string.IsNullOrWhiteSpace(req.Name))
            property.Name = req.Name;
        if (req.Desc != null)
            property.Desc = req.Desc;
        if (req.Area.HasValue)
            property.Area = req.Area.Value;
        if (req.Price.HasValue)
            property.Price = req.Price.Value;
        if (req.Currency.HasValue)
            property.Currency = req.Currency.Value;
        if (req.Period.HasValue)
            property.Period = req.Period.Value;
        if (req.Status.HasValue)
            property.Status = req.Status.Value;

        // Update property attributes if provided
        if (req.PropertyAttributes != null)
        {
            // Remove existing attributes
            _context.Attributes.RemoveRange(property.PropertyAttributes);

            // Add new attributes
            var newAttributes = req
                .PropertyAttributes.Select(attr => new PropertyAttribute
                {
                    PropertyId = property.Id,
                    Name = attr.Name,
                    Value = attr.Value,
                    AttributeType = attr.AttributeType,
                    IsSearchable = attr.IsSearchable,
                })
                .ToList();

            _context.Attributes.AddRange(newAttributes);
        }

        await _context.SaveChangesAsync();

        // Reload property with all related data
        var updatedProperty = await _context
            .Properties.Include(p => p.User)
            .ThenInclude(u => u.Personal)
            .Include(p => p.Address)
            .Include(p => p.ImageLinks)
            .Include(p => p.PropertyAttributes)
            .AsSplitQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        var dtoProperty = new DTOPropertyWithType(
            updatedProperty,
            updatedProperty.User?.Personal?.FirstName,
            updatedProperty.User?.GetProfileLink(),
            false // Owner's own property, not bookmarked
        );

        return Ok(dtoProperty);
    }
}
