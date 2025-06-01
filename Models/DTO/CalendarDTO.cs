using System.ComponentModel.DataAnnotations;
using landlord_be.Models;

namespace landlord_be.Models.DTO
{
    // Request DTOs
    public class CreateCalendarPeriodReqDTO
    {
        public int PropertyId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CalendarState State { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int? AttachedUserId { get; set; }
    }

    public class UpdateCalendarPeriodReqDTO
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CalendarState State { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int? AttachedUserId { get; set; }
    }

    public class GetCalendarPeriodsReqDTO
    {
        public int PropertyId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        // Removed pagination properties
    }

    public class RemoveCalendarPeriodReqDTO
    {
        public int Id { get; set; }
    }

    public class GetUserHistoryReqDTO
    {
        public PropertySortBy SortBy { get; set; } = PropertySortBy.CreatedDesc;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    // Response DTOs
    public class CalendarPeriodDTO
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CalendarState State { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int? AttachedUserId { get; set; }
        public string? AttachedUserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateCalendarPeriodRespDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public CalendarPeriodDTO? CalendarPeriod { get; set; }
    }

    public class UpdateCalendarPeriodRespDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public CalendarPeriodDTO? CalendarPeriod { get; set; }
    }

    public class GetCalendarPeriodsRespDTO
    {
        public bool Success { get; set; }
        public int Count { get; set; }
        public List<CalendarPeriodDTO> Periods { get; set; } = new List<CalendarPeriodDTO>();
    }

    public class RemoveCalendarPeriodRespDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
    }

    public class GetUserHistoryRespDTO
    {
        public int Count { get; set; }
        public List<DTOPropertyWithType> Properties { get; set; } = new();
    }
}
