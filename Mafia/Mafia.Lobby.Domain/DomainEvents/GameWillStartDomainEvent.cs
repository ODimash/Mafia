using Mafia.Shared.Kernel;

namespace Mafia.Lobby.Domain.DomainEvents;

public class GameWillStartDomainEvent : DomainEvent
{
	public GameWillStartDomainEvent(Guid roomId, DateTime startTime)
	{
		RoomId = roomId;
		StartTime = startTime;
	}
	public Guid RoomId { get; }
	public DateTime StartTime { get; }
	
}
