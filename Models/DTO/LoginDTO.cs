namespace landlord_be.Models.DTO
{
    public class LoginSendCodeReqDTO
    {
        public string Number { get; set; } = "";
    }

    public class LoginSendCodeRespDTO
    {
        public bool Success { get; set; }
        public int VerificationId { get; set; }
    }

    public class LoginVerifyCodeReqDTO
    {
        public int VerificationId { get; set; }
        public int Code { get; set; }
    }

    public class LoginVerifyCodeRespDTO
    {
        public bool Success { get; set; }
        public string Token { get; set; } = "";
        public UserInfoDTO? User { get; set; }
    }

    public class UserInfoDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Patronym { get; set; } = "";
        public string Email { get; set; } = "";
    }
}