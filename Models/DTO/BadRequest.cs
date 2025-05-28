using System.ComponentModel.DataAnnotations;

namespace landlord_be.Models.DTO
{
    public class BadRequestMessage
    {
        public BadRequestMessage(string message) {
            Success = false;
            Message = message;
        }


        public bool Success { get; set; }

        public string Message { get; set; } = "";
    }
}
