using landlord_be.Data;
using landlord_be.Models;
using landlord_be.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    //[HttpPost("get_properties_by_search")]
    //public async Task<ActionResult<IEnumerable<Property>>> GetPropertiesByUserId(PropertyGetByUserDTO req)
    //{
    //    bool userExists = await _context.Users.AnyAsync(u => u.Id == req.UserId);
    //    if (!userExists)
    //    {
    //        BadRequest($"No user with id {req.UserId}");
    //    }
    //
    //    return Ok(await _context.Users.Select(u => u.Properties).ToListAsync());
    //}
}