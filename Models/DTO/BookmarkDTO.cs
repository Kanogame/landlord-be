namespace landlord_be.Models.DTO
{
    public class AddBookmarkReqDTO
    {
        public int PropertyId { get; set; }
    }

    public class AddBookmarkRespDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class RemoveBookmarkReqDTO
    {
        public int PropertyId { get; set; }
    }

    public class RemoveBookmarkRespDTO
    {
        public bool Success { get; set; }
    }

    public class GetBookmarksReqDTO
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetBookmarksRespDTO
    {
        public int Count { get; set; }
        public List<DTOPropertyWithType> Properties { get; set; } = new List<DTOPropertyWithType>();
    }

    public class BookmarkStatusReqDTO
    {
        public int PropertyId { get; set; }
    }

    public class BookmarkStatusRespDTO
    {
        public bool IsBookmarked { get; set; }
    }
}
