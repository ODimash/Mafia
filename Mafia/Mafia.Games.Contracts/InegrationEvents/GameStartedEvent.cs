using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Games.Contracts.InegrationEvents;

public class GameStartedEvent : IEvent
{
	public GameStartedEvent(Guid gameId)
	{
		GameId = gameId;
	}
	public Guid GameId { get; }
}
