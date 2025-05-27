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

    [HttpPost("get_properties_by_user_id")]
    public async Task<ActionResult<IEnumerable<Property>>> GetPropertiesByUserId(
        PropertyGetByUserDTO req
    )
    {
        bool userExists = await _context.Users.AnyAsync(u => u.Id == req.UserId);
        if (!userExists)
        {
            BadRequest($"No user with id {req.UserId}");
        }

        return Ok(await _context.Users.Select(u => u.Properties).ToListAsync());
    }

    [HttpPost("get_properties_search")]
    public async Task<ActionResult<IEnumerable<PropertyGetSearchRespDTO>>> GetPropertiesSearch(
        PropertyGetSearchReqDTO req
    )
    {
        IQueryable<Property> query = _context.Properties.AsQueryable();

        if (req.OfferType.HasValue)
        {
            query = query.Where(p => p.OfferTypeId == req.OfferType);
        }

        var totalCount = await query.CountAsync();
        List<PropertyGetSearchRespElDTO> items = await query
            .Where(p => p.Status == PropertyStatus.Active || p.Status == PropertyStatus.RentEnding) // hiding properties that are not displayed
            .Skip((req.PageNumber - 1) * req.PageSize)
            .Take(req.PageSize) // pagination
            .Include(p => p.User)
            .ThenInclude(u => u.Personal)
            .Include(p => p.Address)
            .Include(p => p.ImageLinks)
            .Select(p => new PropertyGetSearchRespElDTO(
                p,
                p.User != null && p.User.Personal != null ? p.User.Personal.FirstName : null,
                p.User != null ? p.User.GetProfileLink() : null
            ))
            .AsNoTracking()
            .ToListAsync();

        return Ok(new PropertyGetSearchRespDTO { Count = totalCount, Properties = items });
    }
}
