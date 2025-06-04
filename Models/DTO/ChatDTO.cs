namespace landlord_be.Models.DTO
{
    public class CreateChatReqDTO
    {
        public int OtherUserId { get; set; }
        public int PropertyId { get; set; }

        public string InitialMessage { get; set; } = "";
    }

    public class CreateChatRespDTO
    {
        public bool Success { get; set; }
        public int ChatId { get; set; }
    }

    public class SendMessageReqDTO
    {
        public int ChatId { get; set; }
        public string Content { get; set; } = "";
    }

    public class SendMessageRespDTO
    {
        public bool Success { get; set; }
        public ChatMessageDTO? Message { get; set; }
    }

    public class ChatDTO
    {
        public int Id { get; set; }
        public int OtherUserId { get; set; }
        public string OtherUserName { get; set; } = "";
        public int PropertyId { get; set; }
        public string PropertyAddress { get; set; } = "";
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsArchived { get; set; }
        public ChatMessageDTO? LastMessage { get; set; }
        public int UnreadCount { get; set; }
    }

    public class ChatMessageDTO
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime SentDate { get; set; }
        public bool IsRead { get; set; }
        public bool IsOwnMessage { get; set; }
    }

    public class GetChatsRespDTO
    {
        public bool Success { get; set; }
        public List<ChatDTO> Chats { get; set; } = new List<ChatDTO>();
    }

    public class GetChatMessagesRespDTO
    {
        public bool Success { get; set; }
        public List<ChatMessageDTO> Messages { get; set; } = new List<ChatMessageDTO>();
    }

    public class ArchiveChatReqDTO
    {
        public int ChatId { get; set; }
        public bool IsArchived { get; set; }
    }

    public class ArchiveChatRespDTO
    {
        public bool Success { get; set; }
    }

    public class MarkMessagesReadReqDTO
    {
        public int ChatId { get; set; }
    }

    public class MarkMessagesReadRespDTO
    {
        public bool Success { get; set; }
    }
}
