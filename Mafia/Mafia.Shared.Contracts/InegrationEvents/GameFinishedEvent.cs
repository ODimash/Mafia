using Mafia.Shared.Contracts.DTOs;
using Mafia.Shared.Contracts.DTOs.Games;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Shared.Contracts.InegrationEvents;

public class GameFinishedEvent : IEvent
{
	public GameFinishedEvent(GameResult gameResult)
	{
		GameResult = gameResult;
	}
	
	public GameResult  GameResult { get; set; }
}
