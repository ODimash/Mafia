namespace Mafia.Games.Domain.Models
{
    public class ChatMessage
{
    public Guid Id { get; private set; }
    public Guid SenderId { get; private set; }
    public string SenderName { get; private set; }
    public string Content { get; private set; }
    public DateTime Timestamp { get; private set; }
    public ChatType Type { get; private set; }

    public ChatMessage(Guid senderId, string senderName, string content, ChatType type)
    {
        Id = Guid.NewGuid();
        SenderId = senderId;
        SenderName = senderName ?? throw new ArgumentNullException(nameof(senderName));
        Content = content ?? throw new ArgumentNullException(nameof(content));
        Type = type;
        Timestamp = DateTime.UtcNow;
    }
}

    

    public enum ChatType
    {
        General,
        Mafia,
        Viewers
    }

    
}
