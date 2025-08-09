using Mafia.Shared.Kernel;

namespace Mafia.Lobby.Domain.DomainEvents;

public class RoomPrivacyChangedDomainEvent : DomainEvent
{
	public RoomPrivacyChangedDomainEvent(Guid roomId, bool isPrivate, string? password)
	{
		RoomId = roomId;
		IsPrivate = isPrivate;
		Password = password;
	}
	public Guid RoomId { get; }
	public bool IsPrivate { get; }
	public string? Password { get; }
}
