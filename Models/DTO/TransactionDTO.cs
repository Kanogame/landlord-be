using landlord_be.Models;

namespace landlord_be.Models.DTO
{
    public class RentBuyPropertyReqDTO
    {
        public int PropertyId { get; set; }
        public TransactionType TransactionType { get; set; } // Rent or Buy
        public string? Notes { get; set; }
    }

    public class EndRentalReqDTO
    {
        public int PropertyId { get; set; }
        public string? Notes { get; set; }
    }

    public class EndRentalRespDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public DateTime? EndDate { get; set; }
    }

    public class RentBuyPropertyRespDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public TransactionResultDTO? Transaction { get; set; }
    }

    public class TransactionResultDTO
    {
        public int PropertyId { get; set; }
        public string PropertyName { get; set; } = "";
        public string PropertyAddress { get; set; } = "";
        public int SellerId { get; set; }
        public string SellerName { get; set; } = "";
        public int BuyerRenterId { get; set; }
        public string BuyerRenterName { get; set; } = "";
        public TransactionType TransactionType { get; set; }
        public PropertyStatus NewPropertyStatus { get; set; }
        public CalendarPeriodDTO? CalendarPeriod { get; set; } // Only for rent transactions
        public DateTime TransactionDate { get; set; }
        public decimal Price { get; set; }
    }

    public enum TransactionType
    {
        Rent = 1,
        Buy = 2,
    }
}
