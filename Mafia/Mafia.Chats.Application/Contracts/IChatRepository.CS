namespace Mafia.Chats.Application.Contracts;
using Mafia.Chats.Domain.Models;
using Mafia.Games.Domain.Models;

public interface IChatRepository
{
    Task<Chat?> GetChatByIdAsync(Guid id);
    Task AddMessageAsync(Guid chatId, ChatMessage message);
    void AddChat(Chat chat);
    
}