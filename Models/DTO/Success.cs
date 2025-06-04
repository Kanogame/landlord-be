using System.ComponentModel.DataAnnotations;

namespace landlord_be.Models.DTO
{
    public class successMessage
    {
        public successMessage(string message)
        {
            Success = true;
            Message = message;
        }

        public bool Success { get; set; }

        public string Message { get; set; } = "";
    }
}
