namespace Mafia.Chats.Application.Service;

using Mafia.Chats.Application.Contracts;
using Mafia.Chats.Domain.Models;
using Mafia.Games.Domain.Models;

public class ChatService
{
    private readonly IChatRepository _repository;

    public ChatService(IChatRepository repository)
    {

        _repository = repository;
    }

    public async Task SendMessage(Guid chatId, ChatMessage message)
    {
        var chat = await _repository.GetChatByIdAsync(chatId);
        if (chat == null)
            throw new Exception("Chat not found");

        chat.AddMessage(message);
        await _repository.AddMessageAsync(chatId, message);
    }

    public async Task<Chat> GetChatByIdAsync(Guid id)
    {
        var chat = await _repository.GetChatByIdAsync(id);
        if (chat == null)
            throw new Exception("Chat not found");
        return chat;
    }
    public async Task<Chat> CreateChatAsync(string chatTitle)
    {
        var chat = new Chat(chatTitle);
        _repository.AddChat(chat);
        return chat;
    }



}