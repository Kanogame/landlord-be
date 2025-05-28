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
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChatController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("create")]
        public async Task<ActionResult<CreateChatRespDTO>> CreateChat(CreateChatReqDTO req)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == null)
            {
                return Unauthorized(new BadRequestMessage("User not authenticated"));
            }

            // Validate that the other user exists
            var otherUserExists = await _context.Users.AnyAsync(u => u.Id == req.OtherUserId);
            if (!otherUserExists)
            {
                return BadRequest(new BadRequestMessage("Other user not found"));
            }

            // Validate that the property exists
            var propertyExists = await _context.Properties.AnyAsync(p => p.Id == req.PropertyId);
            if (!propertyExists)
            {
                return BadRequest(new BadRequestMessage("Property not found"));
            }

            // Check if chat already exists between these users for this property
            var existingChat = await _context.Chats.FirstOrDefaultAsync(c =>
                (
                    (c.User1Id == currentUserId && c.User2Id == req.OtherUserId)
                    || (c.User1Id == req.OtherUserId && c.User2Id == currentUserId)
                )
                && c.PropertyId == req.PropertyId
            );

            if (existingChat != null)
            {
                return Ok(new CreateChatRespDTO { Success = true, ChatId = existingChat.Id });
            }

            // Create new chat
            var newChat = new Chat
            {
                User1Id = currentUserId.Value,
                User2Id = req.OtherUserId,
                PropertyId = req.PropertyId,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
            };

            _context.Chats.Add(newChat);
            await _context.SaveChangesAsync();

            return Ok(new CreateChatRespDTO { Success = true, ChatId = newChat.Id });
        }

        [HttpGet("list")]
        public async Task<ActionResult<GetChatsRespDTO>> GetChats(
            [FromQuery] bool includeArchived = false
        )
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == null)
            {
                return Unauthorized(new BadRequestMessage("User not authenticated"));
            }

            var chatsQuery = _context
                .Chats.Include(c => c.User1)
                .ThenInclude(u => u.Personal)
                .Include(c => c.User2)
                .ThenInclude(u => u.Personal)
                .Include(c => c.Property)
                .ThenInclude(p => p.Address)
                .Include(c => c.Messages.OrderByDescending(m => m.SentDate).Take(1))
                .ThenInclude(m => m.Sender)
                .ThenInclude(s => s.Personal)
                .Where(c => c.User1Id == currentUserId || c.User2Id == currentUserId);

            if (!includeArchived)
            {
                chatsQuery = chatsQuery.Where(c =>
                    (c.User1Id == currentUserId && !c.IsArchivedByUser1)
                    || (c.User2Id == currentUserId && !c.IsArchivedByUser2)
                );
            }

            var chats = await chatsQuery.ToListAsync();

            var chatDTOs = chats
                .Select(c =>
                {
                    var otherUser = c.User1Id == currentUserId ? c.User2 : c.User1;
                    var isArchived =
                        c.User1Id == currentUserId ? c.IsArchivedByUser1 : c.IsArchivedByUser2;
                    var unreadCount = _context.ChatMessages.Count(m =>
                        m.ChatId == c.Id && m.SenderId != currentUserId && !m.IsRead
                    );

                    return new ChatDTO
                    {
                        Id = c.Id,
                        OtherUserId = otherUser?.Id ?? 0,
                        OtherUserName =
                            $"{otherUser?.Personal?.FirstName} {otherUser?.Personal?.LastName}".Trim(),
                        PropertyId = c.PropertyId,
                        PropertyAddress = c.Property?.Address?.DisplayAddress ?? "",
                        CreatedDate = c.CreatedDate,
                        UpdatedDate = c.UpdatedDate,
                        IsArchived = isArchived,
                        UnreadCount = unreadCount,
                        LastMessage =
                            c.Messages.FirstOrDefault() != null
                                ? new ChatMessageDTO
                                {
                                    Id = c.Messages.First().Id,
                                    SenderId = c.Messages.First().SenderId,
                                    SenderName =
                                        $"{c.Messages.First().Sender?.Personal?.FirstName} {c.Messages.First().Sender?.Personal?.LastName}".Trim(),
                                    Content = c.Messages.First().Content,
                                    SentDate = c.Messages.First().SentDate,
                                    IsRead = c.Messages.First().IsRead,
                                    IsOwnMessage = c.Messages.First().SenderId == currentUserId,
                                }
                                : null,
                    };
                })
                .OrderByDescending(c => c.UpdatedDate)
                .ToList();

            return Ok(new GetChatsRespDTO { Success = true, Chats = chatDTOs });
        }

        [HttpGet("{chatId}/messages")]
        public async Task<ActionResult<GetChatMessagesRespDTO>> GetChatMessages(
            int chatId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50
        )
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == null)
            {
                return Unauthorized(new BadRequestMessage("User not authenticated"));
            }

            // Verify user has access to this chat
            var chat = await _context.Chats.FirstOrDefaultAsync(c =>
                c.Id == chatId && (c.User1Id == currentUserId || c.User2Id == currentUserId)
            );

            if (chat == null)
            {
                return BadRequest(new BadRequestMessage("Chat not found or access denied"));
            }

            var messages = await _context
                .ChatMessages.Include(m => m.Sender)
                .ThenInclude(s => s.Personal)
                .Where(m => m.ChatId == chatId)
                .OrderByDescending(m => m.SentDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var messageDTOs = messages
                .Select(m => new ChatMessageDTO
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    SenderName =
                        $"{m.Sender?.Personal?.FirstName} {m.Sender?.Personal?.LastName}".Trim(),
                    Content = m.Content,
                    SentDate = m.SentDate,
                    IsRead = m.IsRead,
                    IsOwnMessage = m.SenderId == currentUserId,
                })
                .Reverse()
                .ToList();

            return Ok(new GetChatMessagesRespDTO { Success = true, Messages = messageDTOs });
        }

        [HttpPost("send-message")]
        public async Task<ActionResult<SendMessageRespDTO>> SendMessage(SendMessageReqDTO req)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == null)
            {
                return Unauthorized(new BadRequestMessage("User not authenticated"));
            }

            if (string.IsNullOrWhiteSpace(req.Content))
            {
                return BadRequest(new BadRequestMessage("Message content is required"));
            }

            if (req.Content.Length > 1000)
            {
                return BadRequest(new BadRequestMessage("Message content is too long"));
            }

            // Verify user has access to this chat
            var chat = await _context.Chats.FirstOrDefaultAsync(c =>
                c.Id == req.ChatId && (c.User1Id == currentUserId || c.User2Id == currentUserId)
            );

            if (chat == null)
            {
                return BadRequest(new BadRequestMessage("Chat not found or access denied"));
            }

            var message = new ChatMessage
            {
                ChatId = req.ChatId,
                SenderId = currentUserId.Value,
                Content = req.Content.Trim(),
                SentDate = DateTime.UtcNow,
                IsRead = false,
            };

            _context.ChatMessages.Add(message);

            // Update chat's UpdatedDate
            chat.UpdatedDate = DateTime.UtcNow;
            _context.Chats.Update(chat);

            await _context.SaveChangesAsync();

            // Load the message with sender info for response
            var messageWithSender = await _context
                .ChatMessages.Include(m => m.Sender)
                .ThenInclude(s => s.Personal)
                .FirstAsync(m => m.Id == message.Id);

            var messageDTO = new ChatMessageDTO
            {
                Id = messageWithSender.Id,
                SenderId = messageWithSender.SenderId,
                SenderName =
                    $"{messageWithSender.Sender?.Personal?.FirstName} {messageWithSender.Sender?.Personal?.LastName}".Trim(),
                Content = messageWithSender.Content,
                SentDate = messageWithSender.SentDate,
                IsRead = messageWithSender.IsRead,
                IsOwnMessage = true,
            };

            return Ok(new SendMessageRespDTO { Success = true, Message = messageDTO });
        }

        [HttpPost("archive")]
        public async Task<ActionResult<ArchiveChatRespDTO>> ArchiveChat(ArchiveChatReqDTO req)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == null)
            {
                return Unauthorized(new BadRequestMessage("User not authenticated"));
            }

            var chat = await _context.Chats.FirstOrDefaultAsync(c =>
                c.Id == req.ChatId && (c.User1Id == currentUserId || c.User2Id == currentUserId)
            );

            if (chat == null)
            {
                return BadRequest(new BadRequestMessage("Chat not found or access denied"));
            }

            // Update archive status for the current user
            if (chat.User1Id == currentUserId)
            {
                chat.IsArchivedByUser1 = req.IsArchived;
            }
            else
            {
                chat.IsArchivedByUser2 = req.IsArchived;
            }

            _context.Chats.Update(chat);
            await _context.SaveChangesAsync();

            return Ok(new ArchiveChatRespDTO { Success = true });
        }

        [HttpPost("mark-read")]
        public async Task<ActionResult<MarkMessagesReadRespDTO>> MarkMessagesAsRead(
            MarkMessagesReadReqDTO req
        )
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == null)
            {
                return Unauthorized(new BadRequestMessage("User not authenticated"));
            }

            // Verify user has access to this chat
            var chat = await _context.Chats.FirstOrDefaultAsync(c =>
                c.Id == req.ChatId && (c.User1Id == currentUserId || c.User2Id == currentUserId)
            );

            if (chat == null)
            {
                return BadRequest(new BadRequestMessage("Chat not found or access denied"));
            }

            // Mark all unread messages from other users as read
            var unreadMessages = await _context
                .ChatMessages.Where(m =>
                    m.ChatId == req.ChatId && m.SenderId != currentUserId && !m.IsRead
                )
                .ToListAsync();

            foreach (var message in unreadMessages)
            {
                message.IsRead = true;
            }

            _context.ChatMessages.UpdateRange(unreadMessages);
            await _context.SaveChangesAsync();

            return Ok(new MarkMessagesReadRespDTO { Success = true });
        }

        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            return null;
        }
    }
}
