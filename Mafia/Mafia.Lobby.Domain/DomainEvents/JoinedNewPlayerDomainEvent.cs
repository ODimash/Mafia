using Mafia.Shared.Kernel;

namespace Mafia.Lobby.Domain.DomainEvents;

public class JoinedNewPlayerDomainEvent : DomainEvent
{
	public JoinedNewPlayerDomainEvent(Guid roomId, Guid identityId, Guid participantId)
	{
		RoomId = roomId;
		IdentityId = identityId;
		ParticipantId = participantId;
	}
	public Guid RoomId { get; }
	public Guid IdentityId { get; }
	public Guid ParticipantId { get; }
}
