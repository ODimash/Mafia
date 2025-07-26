using Mafia.Shared.Kernel;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Games.Domain.Events;

public class GamePhaseTimeEndDomainEvent : DomainEvent
{
	public GamePhaseTimeEndDomainEvent(Guid gameId, PhaseType phase)
	{
		GameId = gameId;
		Phase = phase;
	}
	public Guid GameId { get; }
	public PhaseType Phase { get; set; }
}
