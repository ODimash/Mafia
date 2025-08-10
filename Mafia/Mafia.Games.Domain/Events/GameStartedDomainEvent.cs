using Mafia.Games.Domain.Models;
using Mafia.Shared.Kernel;

namespace Mafia.Games.Domain.Events;

public class GameStartedDomainEvent : DomainEvent
{
	public GameStartedDomainEvent(Guid gameId, List<Player> players)
	{
		GameId = gameId;
		Players = players;
	}

	public Guid GameId { get; }
	public List<Player> Players { get; set; }
}
