using Mafia.Shared.Kernel;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Games.Domain.Events;

public class GamePhaseChangedDomainEvent : DomainEvent
{
	public GamePhaseChangedDomainEvent(Guid gameId, PhaseType oldPhaseType, PhaseType newPhaseType)
	{
		GameId = gameId;
		NewPhaseType = newPhaseType;
		OldPhaseType = oldPhaseType;
	}
	public Guid GameId { get; }
	public PhaseType NewPhaseType { get; }
	public PhaseType OldPhaseType { get; }
}
