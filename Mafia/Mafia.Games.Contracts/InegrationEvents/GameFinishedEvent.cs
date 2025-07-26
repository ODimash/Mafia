using Mafia.Games.Contracts.DTOs;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Games.Contracts.InegrationEvents;

public class GameFinishedEvent : IEvent
{
	public GameFinishedEvent(GameResult gameResult)
	{
		GameResult = gameResult;
	}
	
	public GameResult  GameResult { get; set; }
}
