using Mafia.Games.Domain.Models;
namespace Mafia.Chats.Domain.Models;
public class Chat
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string ChatTitle { get; private set; }
    private readonly List<ChatMessage> _chatMessages = new();
    public IReadOnlyList<ChatMessage> ChatMessages => _chatMessages.AsReadOnly();

    public Chat(string chatTitle)
    {
        ChatTitle = chatTitle ?? throw new ArgumentNullException(nameof(chatTitle));
    }

    public void AddMessage(ChatMessage message)
    {
        _chatMessages.Add(message);
    }
}
