namespace landlord_be.Models.DTO
{
    public class UploadImageRespDTO
    {
        public bool Success { get; set; }
        public string? Url { get; set; }
        public string? FileName { get; set; }
        public string? Message { get; set; }
    }

    public class DeleteImageRespDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
    }
}
