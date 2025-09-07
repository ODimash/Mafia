namespace Mafia.Chats.Infrastructure.Persistence;

using Mafia.Chats.Application.Contracts;
using Mafia.Chats.Domain.Models;
using Mafia.Games.Domain.Models;

public class InMemoryChatRepository : IChatRepository
{
    private readonly Dictionary<Guid, Chat> _chats = new();

    public Task<Chat?> GetChatByIdAsync(Guid chatid)
    {
        _chats.TryGetValue(chatid, out var chat);
        return Task.FromResult(chat);

    }

    public Task AddMessageAsync(Guid chatId, ChatMessage message)
    {
        if (_chats.TryGetValue(chatId, out var chat))
        {
            chat.AddMessage(message);
        }
        return Task.CompletedTask;
    }
        
    public void AddChat(Chat chat)
    {
        _chats[chat.Id] = chat;
    }

    
}