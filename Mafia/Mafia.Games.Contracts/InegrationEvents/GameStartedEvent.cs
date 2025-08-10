using Mafia.Games.Contracts.DTOs;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Games.Contracts.InegrationEvents;

public class GameStartedEvent : IEvent
{
	public GameStartedEvent(Guid gameId, List<PlayerWithRoleDto> playersWithRole)
	{
		GameId = gameId;
		PlayersWithRole = playersWithRole;
	}
	public Guid GameId { get; }
	public List<PlayerWithRoleDto> PlayersWithRole { get; set; }
}
