using Mafia.Shared.Kernel;

namespace Mafia.Games.Domain.Events;

public class GameStartedDomainEvent : DomainEvent
{
	public GameStartedDomainEvent(Guid gameId)
	{
		GameId = gameId;
	}
	
	public Guid GameId { get; }
}
