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
    public class CalendarController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CalendarController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : null;
        }

        [HttpPost("create")]
        public async Task<ActionResult<CreateCalendarPeriodRespDTO>> CreateCalendarPeriod(
            CreateCalendarPeriodReqDTO req
        )
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == null)
            {
                return Unauthorized(new BadRequestMessage("User not authenticated"));
            }

            // Validate property exists and user owns it
            var property = await _context.Properties.FirstOrDefaultAsync(p =>
                p.Id == req.PropertyId
            );
            if (property == null)
            {
                return BadRequest(
                    new CreateCalendarPeriodRespDTO
                    {
                        Success = false,
                        Message = $"Property with id {req.PropertyId} not found",
                    }
                );
            }

            if (property.OwnerId != currentUserId)
            {
                return Forbid();
            }

            // Validate dates
            if (req.StartDate >= req.EndDate)
            {
                return BadRequest(
                    new CreateCalendarPeriodRespDTO
                    {
                        Success = false,
                        Message = "Start date must be before end date",
                    }
                );
            }

            if (req.StartDate < DateTime.UtcNow.Date)
            {
                return BadRequest(
                    new CreateCalendarPeriodRespDTO
                    {
                        Success = false,
                        Message = "Start date cannot be in the past",
                    }
                );
            }

            // Check for overlapping periods
            var hasOverlap = await _context.CalendarPeriods.AnyAsync(cp =>
                cp.PropertyId == req.PropertyId
                && (
                    (req.StartDate >= cp.StartDate && req.StartDate < cp.EndDate)
                    || (req.EndDate > cp.StartDate && req.EndDate <= cp.EndDate)
                    || (req.StartDate <= cp.StartDate && req.EndDate >= cp.EndDate)
                )
            );

            if (hasOverlap)
            {
                return BadRequest(
                    new CreateCalendarPeriodRespDTO
                    {
                        Success = false,
                        Message = "Period overlaps with existing calendar entry",
                    }
                );
            }

            // Validate attached user if provided
            if (req.AttachedUserId.HasValue)
            {
                var userExists = await _context.Users.AnyAsync(u => u.Id == req.AttachedUserId);
                if (!userExists)
                {
                    return BadRequest(
                        new CreateCalendarPeriodRespDTO
                        {
                            Success = false,
                            Message = $"User with id {req.AttachedUserId} not found",
                        }
                    );
                }
            }

            var calendarPeriod = new CalendarPeriod
            {
                PropertyId = req.PropertyId,
                StartDate = req.StartDate,
                EndDate = req.EndDate,
                State = req.State,
                Name = req.Name,
                Description = req.Description,
                AttachedUserId = req.AttachedUserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            _context.CalendarPeriods.Add(calendarPeriod);
            await _context.SaveChangesAsync();

            // Load the created period with user info
            var createdPeriod = await _context
                .CalendarPeriods.Include(cp => cp.AttachedUser)
                .ThenInclude(u => u.Personal)
                .FirstAsync(cp => cp.Id == calendarPeriod.Id);

            var periodDTO = MapToDTO(createdPeriod);

            return Ok(
                new CreateCalendarPeriodRespDTO
                {
                    Success = true,
                    Message = "Calendar period created successfully",
                    CalendarPeriod = periodDTO,
                }
            );
        }

        [HttpPost("update")]
        public async Task<ActionResult<UpdateCalendarPeriodRespDTO>> UpdateCalendarPeriod(
            UpdateCalendarPeriodReqDTO req
        )
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == null)
            {
                return Unauthorized(new BadRequestMessage("User not authenticated"));
            }

            var calendarPeriod = await _context
                .CalendarPeriods.Include(cp => cp.Property)
                .FirstOrDefaultAsync(cp => cp.Id == req.Id);

            if (calendarPeriod == null)
            {
                return BadRequest(
                    new UpdateCalendarPeriodRespDTO
                    {
                        Success = false,
                        Message = "Calendar period not found",
                    }
                );
            }

            if (calendarPeriod.Property?.OwnerId != currentUserId)
            {
                return Forbid();
            }

            // Validate dates
            if (req.StartDate >= req.EndDate)
            {
                return BadRequest(
                    new UpdateCalendarPeriodRespDTO
                    {
                        Success = false,
                        Message = "Start date must be before end date",
                    }
                );
            }

            // Check for overlapping periods (excluding current period)
            var hasOverlap = await _context.CalendarPeriods.AnyAsync(cp =>
                cp.PropertyId == calendarPeriod.PropertyId
                && cp.Id != req.Id
                && (
                    (req.StartDate >= cp.StartDate && req.StartDate < cp.EndDate)
                    || (req.EndDate > cp.StartDate && req.EndDate <= cp.EndDate)
                    || (req.StartDate <= cp.StartDate && req.EndDate >= cp.EndDate)
                )
            );

            if (hasOverlap)
            {
                return BadRequest(
                    new UpdateCalendarPeriodRespDTO
                    {
                        Success = false,
                        Message = "Period overlaps with existing calendar entry",
                    }
                );
            }

            // Validate attached user if provided
            if (req.AttachedUserId.HasValue)
            {
                var userExists = await _context.Users.AnyAsync(u => u.Id == req.AttachedUserId);
                if (!userExists)
                {
                    return BadRequest(
                        new UpdateCalendarPeriodRespDTO
                        {
                            Success = false,
                            Message = $"User with id {req.AttachedUserId} not found",
                        }
                    );
                }
            }

            calendarPeriod.StartDate = req.StartDate;
            calendarPeriod.EndDate = req.EndDate;
            calendarPeriod.State = req.State;
            calendarPeriod.Name = req.Name;
            calendarPeriod.Description = req.Description;
            calendarPeriod.AttachedUserId = req.AttachedUserId;
            calendarPeriod.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Reload with user info
            var updatedPeriod = await _context
                .CalendarPeriods.Include(cp => cp.AttachedUser)
                .ThenInclude(u => u.Personal)
                .FirstAsync(cp => cp.Id == calendarPeriod.Id);

            var periodDTO = MapToDTO(updatedPeriod);

            return Ok(
                new UpdateCalendarPeriodRespDTO
                {
                    Success = true,
                    Message = "Calendar period updated successfully",
                    CalendarPeriod = periodDTO,
                }
            );
        }

        [HttpPost("get")]
        public async Task<ActionResult<GetCalendarPeriodsRespDTO>> GetCalendarPeriods(
            GetCalendarPeriodsReqDTO req
        )
        {
            // Validate property exists
            var property = await _context.Properties.FirstOrDefaultAsync(p =>
                p.Id == req.PropertyId
            );
            if (property == null)
            {
                return BadRequest(new GetCalendarPeriodsRespDTO { Success = false });
            }

            IQueryable<CalendarPeriod> query = _context
                .CalendarPeriods.Where(cp => cp.PropertyId == req.PropertyId)
                .Include(cp => cp.AttachedUser)
                .ThenInclude(u => u.Personal);

            // Apply date filters if provided
            if (req.StartDate.HasValue)
            {
                query = query.Where(cp => cp.EndDate >= req.StartDate.Value);
            }

            if (req.EndDate.HasValue)
            {
                query = query.Where(cp => cp.StartDate <= req.EndDate.Value);
            }

            query = query.OrderBy(cp => cp.StartDate);

            var periods = await query.AsNoTracking().ToListAsync();

            var periodDTOs = periods.Select(MapToDTO).ToList();

            return Ok(
                new GetCalendarPeriodsRespDTO
                {
                    Success = true,
                    Count = periodDTOs.Count,
                    Periods = periodDTOs,
                }
            );
        }

        [HttpPost("remove")]
        public async Task<ActionResult<RemoveCalendarPeriodRespDTO>> RemoveCalendarPeriod(
            RemoveCalendarPeriodReqDTO req
        )
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == null)
            {
                return Unauthorized(new BadRequestMessage("User not authenticated"));
            }

            var calendarPeriod = await _context
                .CalendarPeriods.Include(cp => cp.Property)
                .FirstOrDefaultAsync(cp => cp.Id == req.Id);

            if (calendarPeriod == null)
            {
                return BadRequest(
                    new RemoveCalendarPeriodRespDTO
                    {
                        Success = false,
                        Message = "Calendar period not found",
                    }
                );
            }

            if (calendarPeriod.Property?.OwnerId != currentUserId)
            {
                return Forbid();
            }

            _context.CalendarPeriods.Remove(calendarPeriod);
            await _context.SaveChangesAsync();

            return Ok(
                new RemoveCalendarPeriodRespDTO
                {
                    Success = true,
                    Message = "Calendar period removed successfully",
                }
            );
        }

        [HttpPost("get_user_history")]
        [Authorize]
        public async Task<ActionResult<GetUserHistoryRespDTO>> GetUserHistory(
            GetUserHistoryReqDTO req
        )
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == null)
            {
                return Unauthorized(new BadRequestMessage("User not authenticated"));
            }

            IQueryable<CalendarPeriod> query = _context
                .CalendarPeriods.Where(cp =>
                    cp.AttachedUserId == currentUserId
                    && (cp.State == CalendarState.Rented || cp.State == CalendarState.Sold)
                )
                .Include(cp => cp.Property)
                .ThenInclude(p => p.User)
                .ThenInclude(u => u.Personal)
                .Include(cp => cp.Property)
                .ThenInclude(p => p.Address)
                .Include(cp => cp.Property)
                .ThenInclude(p => p.ImageLinks)
                .Include(cp => cp.Property)
                .ThenInclude(p => p.PropertyAttributes);

            // Apply sorting based on calendar period creation date or property attributes
            query = req.SortBy switch
            {
                PropertySortBy.PriceAsc => query.OrderBy(cp => cp.Property.Price),
                PropertySortBy.PriceDesc => query.OrderByDescending(cp => cp.Property.Price),
                PropertySortBy.AreaAsc => query.OrderBy(cp => cp.Property.Area),
                PropertySortBy.AreaDesc => query.OrderByDescending(cp => cp.Property.Area),
                PropertySortBy.CreatedAsc => query.OrderBy(cp => cp.CreatedAt),
                PropertySortBy.CreatedDesc => query.OrderByDescending(cp => cp.CreatedAt),
                _ => query.OrderByDescending(cp => cp.CreatedAt),
            };

            var totalCount = await query.CountAsync();

            var calendarPeriods = await query
                .Skip((req.PageNumber - 1) * req.PageSize)
                .Take(req.PageSize)
                .AsSplitQuery()
                .AsNoTracking()
                .ToListAsync();

            var properties = calendarPeriods
                .Select(cp => new DTOPropertyWithType(
                    cp.Property,
                    cp.Property.User?.Personal?.FirstName,
                    cp.Property.User?.GetProfileLink(),
                    false // Not bookmarked in this context, showing rental/purchase history
                ))
                .ToList();

            return Ok(new GetUserHistoryRespDTO { Count = totalCount, Properties = properties });
        }

        private static CalendarPeriodDTO MapToDTO(CalendarPeriod period)
        {
            return new CalendarPeriodDTO
            {
                Id = period.Id,
                PropertyId = period.PropertyId,
                StartDate = period.StartDate,
                EndDate = period.EndDate,
                State = period.State,
                Name = period.Name,
                Description = period.Description,
                AttachedUserId = period.AttachedUserId,
                AttachedUserName = period.AttachedUser?.Personal?.FirstName ?? "",
                CreatedAt = period.CreatedAt,
                UpdatedAt = period.UpdatedAt,
            };
        }
    }
}
