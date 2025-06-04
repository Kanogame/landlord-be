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
    public class CalendarController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : null;
        }

        [HttpPost("rent-buy-property")]
        [Authorize]
        public async Task<ActionResult<RentBuyPropertyRespDTO>> RentBuyProperty(
            RentBuyPropertyReqDTO req
        )
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == null)
            {
                return Unauthorized(new BadRequestMessage("Пользователь не авторизован"));
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Validate property exists and is active
                var property = await _context
                    .Properties.Include(p => p.User)
                    .ThenInclude(u => u.Personal)
                    .Include(p => p.Address)
                    .FirstOrDefaultAsync(p => p.Id == req.PropertyId);

                if (property == null)
                {
                    return BadRequest(
                        new RentBuyPropertyRespDTO
                        {
                            Success = false,
                            Message = $"Недвижимость с id {req.PropertyId} не найдена",
                        }
                    );
                }

                if (
                    property.Status != PropertyStatus.Active
                    && property.Status != PropertyStatus.RentEnding
                )
                {
                    return BadRequest(
                        new RentBuyPropertyRespDTO
                        {
                            Success = false,
                            Message = "Недвижимость недоступна для аренды или покупки",
                        }
                    );
                }

                // Validate buyer/renter exists
                var buyerRenter = await _context
                    .Users.Include(u => u.Personal)
                    .FirstOrDefaultAsync(u => u.Id == currentUserId);

                if (buyerRenter == null)
                {
                    return BadRequest(
                        new RentBuyPropertyRespDTO
                        {
                            Success = false,
                            Message = $"Пользователь с id {currentUserId} не найден",
                        }
                    );
                }

                // Validate that buyer/renter is not the owner
                if (property.OwnerId == currentUserId)
                {
                    return BadRequest(
                        new RentBuyPropertyRespDTO
                        {
                            Success = false,
                            Message =
                                "Владелец не может арендовать или купить свою собственную недвижимость",
                        }
                    );
                }

                // Validate transaction type matches property offer type
                if (
                    req.TransactionType == TransactionType.Rent
                    && property.OfferTypeId != OfferType.Rent
                )
                {
                    return BadRequest(
                        new RentBuyPropertyRespDTO
                        {
                            Success = false,
                            Message = "Недвижимость недоступна для аренды",
                        }
                    );
                }

                if (
                    req.TransactionType == TransactionType.Buy
                    && property.OfferTypeId != OfferType.Sell
                )
                {
                    return BadRequest(
                        new RentBuyPropertyRespDTO
                        {
                            Success = false,
                            Message = "Недвижимость недоступна для продажи",
                        }
                    );
                }

                CalendarPeriodDTO? calendarPeriodDTO = null;

                // Handle rental transaction
                if (req.TransactionType == TransactionType.Rent)
                {
                    // Check if property is already being rented (has active rental period)
                    var existingRental = await _context.CalendarPeriods.AnyAsync(cp =>
                        cp.PropertyId == req.PropertyId
                        && cp.State == CalendarState.Rented
                        && cp.StartDate <= DateTime.UtcNow.Date
                        && cp.EndDate > DateTime.UtcNow.Date
                    );

                    if (existingRental)
                    {
                        return BadRequest(
                            new RentBuyPropertyRespDTO
                            {
                                Success = false,
                                Message = "Недвижимость уже сдается в аренду",
                            }
                        );
                    }

                    // Create calendar period for rental starting today and ending far in the future
                    // We'll use a date far in the future (e.g., 10 years) as a placeholder
                    var startDate = DateTime.UtcNow.Date;
                    var endDate = DateTime.UtcNow.Date.AddYears(10); // Placeholder end date

                    var calendarPeriod = new CalendarPeriod
                    {
                        PropertyId = req.PropertyId,
                        StartDate = startDate,
                        EndDate = endDate,
                        State = CalendarState.Rented,
                        Name = $"Аренда - {buyerRenter.Personal?.FirstName ?? "Арендатор"}",
                        Description =
                            req.Notes
                            ?? $"Сдано в аренду пользователю {buyerRenter.Personal?.FirstName} {buyerRenter.Personal?.LastName}. Аренда началась {startDate:dd.MM.yyyy}",
                        AttachedUserId = currentUserId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    };

                    _context.CalendarPeriods.Add(calendarPeriod);
                    await _context.SaveChangesAsync();

                    calendarPeriodDTO = MapToDTO(calendarPeriod);
                    calendarPeriodDTO.AttachedUserName = buyerRenter.Personal?.FirstName ?? "";

                    // Update property status to rented
                    property.Status = PropertyStatus.Rented;
                }
                else // Buy transaction
                {
                    // Create calendar period for purchase record
                    var purchaseDate = DateTime.UtcNow.Date;

                    var calendarPeriod = new CalendarPeriod
                    {
                        PropertyId = req.PropertyId,
                        StartDate = purchaseDate,
                        EndDate = purchaseDate, // Same day for purchase
                        State = CalendarState.Sold,
                        Name = $"Покупка - {buyerRenter.Personal?.FirstName ?? "Покупатель"}",
                        Description =
                            req.Notes
                            ?? $"Приобретено пользователем {buyerRenter.Personal?.FirstName} {buyerRenter.Personal?.LastName}. Покупка совершена {purchaseDate:dd.MM.yyyy}",
                        AttachedUserId = currentUserId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    };

                    _context.CalendarPeriods.Add(calendarPeriod);
                    await _context.SaveChangesAsync();

                    calendarPeriodDTO = MapToDTO(calendarPeriod);
                    calendarPeriodDTO.AttachedUserName = buyerRenter.Personal?.FirstName ?? "";

                    // Update property status to sold and change owner
                    property.Status = PropertyStatus.Sold;
                    property.OwnerId = currentUserId ?? -1;
                }

                property.UpdatedAt = DateTime.UtcNow;
                _context.Properties.Update(property);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                // Prepare response
                var transactionResult = new TransactionResultDTO
                {
                    PropertyId = property.Id,
                    PropertyName = property.Name,
                    PropertyAddress = property.Address?.DisplayAddress ?? "",
                    SellerId =
                        req.TransactionType == TransactionType.Buy
                            ? property.OwnerId
                            : property.OwnerId,
                    SellerName =
                        property.User?.Personal != null
                            ? $"{property.User.Personal.FirstName} {property.User.Personal.LastName}".Trim()
                            : "",
                    BuyerRenterId = currentUserId ?? -1,
                    BuyerRenterName =
                        buyerRenter.Personal != null
                            ? $"{buyerRenter.Personal.FirstName} {buyerRenter.Personal.LastName}".Trim()
                            : "",
                    TransactionType = req.TransactionType,
                    NewPropertyStatus = property.Status,
                    CalendarPeriod = calendarPeriodDTO,
                    TransactionDate = DateTime.UtcNow,
                    Price = property.Price,
                };

                string successMessage =
                    req.TransactionType == TransactionType.Rent
                        ? "Недвижимость успешно арендована. Период аренды начался сегодня."
                        : "Недвижимость успешно приобретена";

                return Ok(
                    new RentBuyPropertyRespDTO
                    {
                        Success = true,
                        Message = successMessage,
                        Transaction = transactionResult,
                    }
                );
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return StatusCode(
                    500,
                    new RentBuyPropertyRespDTO
                    {
                        Success = false,
                        Message = "Произошла ошибка при обработке транзакции",
                    }
                );
            }
        }

        [HttpPost("end-rental")]
        [Authorize]
        public async Task<ActionResult<EndRentalRespDTO>> EndRental(EndRentalReqDTO req)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == null)
            {
                return Unauthorized(new BadRequestMessage("Пользователь не авторизован"));
            }

            // Validate property exists and user owns it
            var property = await _context.Properties.FirstOrDefaultAsync(p =>
                p.Id == req.PropertyId
            );

            if (property == null)
            {
                return BadRequest(
                    new EndRentalRespDTO
                    {
                        Success = false,
                        Message = $"Недвижимость с id {req.PropertyId} не найдена",
                    }
                );
            }

            if (property.OwnerId != currentUserId)
            {
                return Forbid("Вы можете завершить аренду только для своей недвижимости");
            }

            if (property.Status != PropertyStatus.Rented)
            {
                return BadRequest(
                    new EndRentalRespDTO
                    {
                        Success = false,
                        Message = "Недвижимость в настоящее время не сдается в аренду",
                    }
                );
            }

            // Find active rental period
            var activeRental = await _context.CalendarPeriods.FirstOrDefaultAsync(cp =>
                cp.PropertyId == req.PropertyId
                && cp.State == CalendarState.Rented
                && cp.StartDate <= DateTime.UtcNow.Date
                && cp.EndDate > DateTime.UtcNow.Date
            );

            if (activeRental == null)
            {
                return BadRequest(
                    new EndRentalRespDTO
                    {
                        Success = false,
                        Message = "Активный период аренды для этой недвижимости не найден",
                    }
                );
            }

            // End the rental period today
            var endDate = DateTime.UtcNow.Date;
            activeRental.EndDate = endDate;
            activeRental.UpdatedAt = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(req.Notes))
            {
                activeRental.Description += $" Аренда завершена {endDate:dd.MM.yyyy}. {req.Notes}";
            }
            else
            {
                activeRental.Description += $" Аренда завершена {endDate:dd.MM.yyyy}.";
            }

            // Update property status back to active
            property.Status = PropertyStatus.Active;
            property.UpdatedAt = DateTime.UtcNow;

            _context.CalendarPeriods.Update(activeRental);
            _context.Properties.Update(property);
            await _context.SaveChangesAsync();

            return Ok(
                new EndRentalRespDTO
                {
                    Success = true,
                    Message = "Аренда успешно завершена. Недвижимость снова доступна.",
                    EndDate = endDate,
                }
            );
        }

        [HttpPost("create")]
        public async Task<ActionResult<CreateCalendarPeriodRespDTO>> CreateCalendarPeriod(
            CreateCalendarPeriodReqDTO req
        )
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == null)
            {
                return Unauthorized(new BadRequestMessage("Пользователь не авторизован"));
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
                        Message = $"Недвижимость с id {req.PropertyId} не найдена",
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
                        Message = "Дата начала должна быть раньше даты окончания",
                    }
                );
            }

            if (req.StartDate < DateTime.UtcNow.Date)
            {
                return BadRequest(
                    new CreateCalendarPeriodRespDTO
                    {
                        Success = false,
                        Message = "Дата начала не может быть в прошлом",
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
                        Message = "Период пересекается с существующей записью в календаре",
                    }
                );
            }

            var calendarPeriod = new CalendarPeriod
            {
                PropertyId = req.PropertyId,
                StartDate = req.StartDate,
                EndDate = req.EndDate,
                State = req.State,
                Name = req.Name,
                Description = req.Description,
                AttachedUserId = null, // Set to null since we're not accepting it from request
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            _context.CalendarPeriods.Add(calendarPeriod);
            await _context.SaveChangesAsync();

            // Load the created period (no need to include user info since AttachedUserId is null)
            var createdPeriod = await _context.CalendarPeriods.FirstAsync(cp =>
                cp.Id == calendarPeriod.Id
            );

            var periodDTO = MapToDTO(createdPeriod);

            return Ok(
                new CreateCalendarPeriodRespDTO
                {
                    Success = true,
                    Message = "Период календаря успешно создан",
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
                return Unauthorized(new BadRequestMessage("Пользователь не авторизован"));
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
                        Message = "Период календаря не найден",
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
                        Message = "Дата начала должна быть раньше даты окончания",
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
                        Message = "Период пересекается с существующей записью в календаре",
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
                            Message = $"Пользователь с id {req.AttachedUserId} не найден",
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
                    Message = "Период календаря успешно обновлен",
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
                return BadRequest(
                    new GetCalendarPeriodsRespDTO
                    {
                        Success = false,
                        Message = $"Недвижимость с id {req.PropertyId} не найдена",
                    }
                );
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
                    Message = "",
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
                return Unauthorized(new BadRequestMessage("Пользователь не авторизован"));
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
                        Message = "Период календаря не найден",
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
                    Message = "Период календаря успешно удален",
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
                return Unauthorized(new BadRequestMessage("Пользователь не авторизован"));
            }

            // Modified query to include both rental history and purchase history
            IQueryable<CalendarPeriod> rentalQuery = _context
                .CalendarPeriods.Where(cp =>
                    cp.AttachedUserId == currentUserId && cp.State == CalendarState.Rented
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

            // Query for purchased properties (where user became the owner)
            IQueryable<Property> purchaseQuery = _context
                .Properties.Where(p =>
                    p.OwnerId == currentUserId
                    && p.Status == PropertyStatus.Sold
                    && p.CreatedAt != p.UpdatedAt // Property was transferred, not originally created by user
                )
                .Include(p => p.User) // Original owner
                .ThenInclude(u => u.Personal)
                .Include(p => p.Address)
                .Include(p => p.ImageLinks)
                .Include(p => p.PropertyAttributes);

            // Get rental periods
            var rentalPeriods = await rentalQuery.AsNoTracking().ToListAsync();

            // Get purchased properties
            var purchasedProperties = await purchaseQuery.AsNoTracking().ToListAsync();

            // Combine both lists with transaction date for sorting
            var combinedHistory =
                new List<(Property Property, DateTime TransactionDate, bool IsRental)>();

            // Add rental history
            combinedHistory.AddRange(rentalPeriods.Select(cp => (cp.Property, cp.CreatedAt, true)));

            // Add purchase history
            combinedHistory.AddRange(purchasedProperties.Select(p => (p, p.UpdatedAt, false)));

            // Apply sorting
            var sortedHistory = req.SortBy switch
            {
                PropertySortBy.PriceAsc => combinedHistory.OrderBy(h => h.Property.Price),
                PropertySortBy.PriceDesc => combinedHistory.OrderByDescending(h =>
                    h.Property.Price
                ),
                PropertySortBy.AreaAsc => combinedHistory.OrderBy(h => h.Property.Area),
                PropertySortBy.AreaDesc => combinedHistory.OrderByDescending(h => h.Property.Area),
                PropertySortBy.CreatedAsc => combinedHistory.OrderBy(h => h.TransactionDate),
                PropertySortBy.CreatedDesc => combinedHistory.OrderByDescending(h =>
                    h.TransactionDate
                ),
                _ => combinedHistory.OrderByDescending(h => h.TransactionDate),
            };

            var totalCount = combinedHistory.Count;

            var pagedHistory = sortedHistory
                .Skip((req.PageNumber - 1) * req.PageSize)
                .Take(req.PageSize)
                .ToList();

            var properties = pagedHistory
                .Select(h => new DTOPropertyWithType(
                    h.Property,
                    h.Property.User?.Personal?.FirstName,
                    h.Property.User?.GetProfileLink(),
                    false // Not bookmarked in this context
                ))
                .ToList();

            return Ok(
                new GetUserHistoryRespDTO
                {
                    Success = true,
                    Message = "",
                    Count = totalCount,
                    Properties = properties,
                }
            );
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
