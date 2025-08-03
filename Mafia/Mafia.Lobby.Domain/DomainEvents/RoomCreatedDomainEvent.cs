using Mafia.Shared.Kernel;

namespace Mafia.Lobby.Domain.DomainEvents;

public class RoomCreatedDomainEvent : DomainEvent
{
	public Guid RoomId { get; init; }

	public RoomCreatedDomainEvent(Guid roomId)
	{
		RoomId = roomId;
	}
}
