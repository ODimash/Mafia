using Mafia.Shared.Kernel;

namespace Mafia.Lobby.Domain.DomainEvents;

public class PlayerLeftRoomDomainEvent : DomainEvent
{
	public Guid RoomId { get; }
	public Guid IdentityId { get; }
	
	public PlayerLeftRoomDomainEvent(Guid roomId, Guid identityId)
	{
		RoomId = roomId;
		IdentityId = identityId;
	}
}
