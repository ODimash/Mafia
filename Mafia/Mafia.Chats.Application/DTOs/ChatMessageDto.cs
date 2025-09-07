namespace Mafia.Chats.Application.DTOs;
 public record ChatMessageDto(Guid Id, Guid SenderId, string SenderName, string Content, DateTime Timestamp, string Type);
