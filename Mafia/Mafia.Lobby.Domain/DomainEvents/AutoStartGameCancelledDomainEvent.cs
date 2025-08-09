using Mafia.Shared.Kernel;

namespace Mafia.Lobby.Domain.DomainEvents;

public class AutoStartGameCancelledDomainEvent : DomainEvent
{
	public AutoStartGameCancelledDomainEvent(Guid roomId)
	{
		RoomId = roomId;
	}
	public Guid RoomId { get; }
}
