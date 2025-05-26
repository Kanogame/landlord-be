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
    public async Task<ActionResult<IEnumerable<Property>>> GetPropertiesByUserId(PropertyGetByUserDTO req)
    {
        bool userExists = await _context.Users.AnyAsync(u => u.Id == req.UserId);
        if (!userExists)
        {
            BadRequest($"No user with id {req.UserId}");
        }

        return Ok(await _context.Users.Select(u => u.Properties).ToListAsync());
    }

    [HttpPost("get_properties_search")]
    public async Task<ActionResult<IEnumerable<Property>>> GetPropertiesSearch(PropertyGetSearchReqDTO req)
    {
        IQueryable<Property> query = _context.Properties.AsQueryable();

        if (req.OfferType.HasValue)
        {
            query = query.Where(p => p.OfferTypeId == req.OfferType);
        }

        var totalCount = await query.CountAsync();
        var items = await query
                .Skip((req.PageNumber - 1) * req.PageSize)
                .Take(req.PageSize)
                .Include(p => p.Address)
                .Include(p => p.ImageLinks)
                .Include(p => p.User)
	            .ThenInclude(
                .AsNoTracking()
                .ToListAsync();


        return Ok(items);
    }
}
