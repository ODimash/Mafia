using Mafia.Shared.Kernel;

namespace Mafia.Lobby.Domain.DomainEvents;

public class PlayerKickedDomainEvent : DomainEvent
{
	public PlayerKickedDomainEvent(Guid roomId, Guid identityId)
	{
		RoomId = roomId;
		IdentityId = identityId;
	}
	public Guid RoomId { get; }
	public Guid IdentityId { get; }
}
