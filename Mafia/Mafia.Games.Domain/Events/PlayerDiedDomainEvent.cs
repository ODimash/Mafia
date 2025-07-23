using Mafia.Shared.Kernel;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Games.Domain.Events;

public class PlayerDiedDomainEvent : DomainEvent
{
	public PlayerDiedDomainEvent(Guid playerId, Guid gameId, PhaseType diedPhaseType, DeathReason deathReason, Guid? killedBy = null)
	{
		PlayerId = playerId;
		GameId = gameId;
		DiedPhaseTyp = diedPhaseType;
		KilledBy = killedBy;
		DeathReason = deathReason;
	}
	public PhaseType DiedPhaseTyp { get; }
	public Guid PlayerId { get; }
	public DeathReason DeathReason { get; }
	public Guid? KilledBy { get; }
	public Guid GameId { get; }
}
