using Mafia.Shared.Kernel;

namespace Mafia.Lobby.Domain.DomainEvents;

public class TimerToStartGameFinishedDomainEvent : DomainEvent
{
	public TimerToStartGameFinishedDomainEvent(Guid roomId)
	{
		RoomId = roomId;
	}
	public Guid RoomId { get; }
}
