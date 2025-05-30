using landlord_be.Data;
using landlord_be.Models;
using landlord_be.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace landlord_be.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertyController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PropertyController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("get_search_attributes")]
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
    public async Task<ActionResult<IEnumerable<Property>>> GetPropertiesByUserId(
        PropertyGetByUserDTO req
    )
    {
        bool userExists = await _context.Users.AnyAsync(u => u.Id == req.UserId);
        if (!userExists)
        {
            return BadRequest(new BadRequestMessage($"No user with id {req.UserId}"));
        }

        return Ok(await _context.Users.Select(u => u.Properties).ToListAsync());
    }

    [HttpPost("get_property_by_id")]
    public async Task<ActionResult<DTOProperty>> GetPropertyById(PropertyGetByIdDTO req)
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

        var dtoProperty = new DTOPropertyWithType(
            property,
            property.User?.Personal?.FirstName,
            property.User?.GetProfileLink()
        );

        return Ok(dtoProperty);
    }

    [HttpPost("get_properties_search")]
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
        var items = await query
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
            .Select(p => new DTOPropertyWithType(
                p,
                p.User != null && p.User.Personal != null ? p.User.Personal.FirstName : null,
                p.User != null ? p.User.GetProfileLink() : null
            ))
            .ToListAsync();

        return Ok(new PropertyGetSearchRespDTO { Count = totalCount, Properties = items });
    }
}
