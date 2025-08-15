using Mafia.Shared.Contracts.Messaging;
using Mafia.Shared.Kernel;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Shared.Contracts.InegrationEvents;

public class PhaseActionsAppliedEvent : IEvent
{
	public PhaseActionsAppliedEvent(Guid gameId, PhaseType phase, List<IDomainEvent> events)
	{
		GameId = gameId;
		Phase = phase;
		ActionsEvents = events;
	}
	
	public Guid GameId { get; }
	public List<IDomainEvent> ActionsEvents { get; }
	public PhaseType Phase { get; }
}
