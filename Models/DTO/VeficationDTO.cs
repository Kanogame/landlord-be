using System.ComponentModel.DataAnnotations;

namespace landlord_be.Models.DTO
{
    public class VerificationNumberReqDTO
    {
        public string Number { get; set; } = "";
    }

    public class VerificationNumberRespDTO
    {
        public bool Success { get; set; }
        public int VerificationId { get; set; }
    }

    public class VerificationCodeReqDTO
    {
        public int VerificationId { get; set; }
        public int Code { get; set; }
    }

    public class VerificationCodeRespDTO
    {
        public bool Success { get; set; }
    }

    public class VerificationPersonalReqDTO
    {
        public int VerificationId { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Patronym { get; set; } = "";
        public string Email { get; set; } = "";
    }

    public class VerificationPersonalRespDTO
    {
        public bool Success { get; set; }
        public string Token { get; set; } = "";
    }
}
