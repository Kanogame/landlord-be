using System.Security.Claims;
using landlord_be.Data;
using landlord_be.Models;
using landlord_be.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace landlord_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookmarkController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : null;
        }

        [HttpPost("add")]
        public async Task<ActionResult<AddBookmarkRespDTO>> AddBookmark(AddBookmarkReqDTO req)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == null)
            {
                return Unauthorized(new BadRequestMessage("User not authenticated"));
            }

            // Check if property exists
            var propertyExists = await _context.Properties.AnyAsync(p => p.Id == req.PropertyId);
            if (!propertyExists)
            {
                return BadRequest(
                    new AddBookmarkRespDTO
                    {
                        Success = false,
                        Message = $"Property with id {req.PropertyId} not found",
                    }
                );
            }

            // Check if bookmark already exists
            var existingBookmark = await _context.Bookmarks.FirstOrDefaultAsync(b =>
                b.UserId == currentUserId && b.PropertyId == req.PropertyId
            );

            if (existingBookmark != null)
            {
                return BadRequest(
                    new AddBookmarkRespDTO
                    {
                        Success = false,
                        Message = "Property is already bookmarked",
                    }
                );
            }
            Console.WriteLine(currentUserId.Value);

            // Create new bookmark
            var bookmark = new Bookmark
            {
                UserId = currentUserId.Value,
                PropertyId = req.PropertyId,
                CreatedAt = DateTime.UtcNow,
            };

            _context.Bookmarks.Add(bookmark);
            await _context.SaveChangesAsync();

            return Ok(
                new AddBookmarkRespDTO
                {
                    Success = true,
                    Message = "Property bookmarked successfully",
                }
            );
        }

        [HttpPost("remove")]
        public async Task<ActionResult<RemoveBookmarkRespDTO>> RemoveBookmark(
            RemoveBookmarkReqDTO req
        )
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == null)
            {
                return Unauthorized(new BadRequestMessage("User not authenticated"));
            }

            var bookmark = await _context.Bookmarks.FirstOrDefaultAsync(b =>
                b.UserId == currentUserId && b.PropertyId == req.PropertyId
            );

            if (bookmark == null)
            {
                return BadRequest(new RemoveBookmarkRespDTO { Success = false });
            }

            _context.Bookmarks.Remove(bookmark);
            await _context.SaveChangesAsync();

            return Ok(new RemoveBookmarkRespDTO { Success = true });
        }

        [HttpPost("get_bookmarks")]
        public async Task<ActionResult<GetBookmarksRespDTO>> GetBookmarks(GetBookmarksReqDTO req)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == null)
            {
                return Unauthorized(new BadRequestMessage("User not authenticated"));
            }

            IQueryable<Bookmark> query = _context
                .Bookmarks.Where(b => b.UserId == currentUserId)
                .Include(b => b.Property)
                .ThenInclude(p => p.User)
                .ThenInclude(u => u.Personal)
                .Include(b => b.Property)
                .ThenInclude(p => p.Address)
                .Include(b => b.Property)
                .ThenInclude(p => p.ImageLinks)
                .Include(b => b.Property)
                .ThenInclude(p => p.PropertyAttributes);

            // Apply sorting
            query = req.SortBy switch
            {
                PropertySortBy.PriceAsc => query.OrderBy(b => b.Property.Price),
                PropertySortBy.PriceDesc => query.OrderByDescending(b => b.Property.Price),
                PropertySortBy.AreaAsc => query.OrderBy(b => b.Property.Area),
                PropertySortBy.AreaDesc => query.OrderByDescending(b => b.Property.Area),
                PropertySortBy.CreatedAsc => query.OrderBy(b => b.CreatedAt),
                PropertySortBy.CreatedDesc => query.OrderByDescending(b => b.CreatedAt),
                _ => query.OrderByDescending(b => b.CreatedAt),
            };

            var totalCount = await query.CountAsync();

            var bookmarks = await query
                .Skip((req.PageNumber - 1) * req.PageSize)
                .Take(req.PageSize)
                .AsSplitQuery()
                .AsNoTracking()
                .ToListAsync();

            var properties = bookmarks
                .Select(b => new DTOPropertyWithType(
                    b.Property,
                    b.Property.User?.Personal?.FirstName,
                    b.Property.User?.GetProfileLink(),
                    true
                ))
                .ToList();

            return Ok(new GetBookmarksRespDTO { Count = totalCount, Properties = properties });
        }

        [HttpPost("check_status")]
        public async Task<ActionResult<BookmarkStatusRespDTO>> CheckBookmarkStatus(
            BookmarkStatusReqDTO req
        )
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == null)
            {
                return Unauthorized(new BadRequestMessage("User not authenticated"));
            }

            var isBookmarked = await _context.Bookmarks.AnyAsync(b =>
                b.UserId == currentUserId && b.PropertyId == req.PropertyId
            );

            return Ok(new BookmarkStatusRespDTO { IsBookmarked = isBookmarked });
        }
    }
}
