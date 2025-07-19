using Mafia.Games.Domain.ValueObjects;
using Mafia.Shared.Kernel;

namespace Mafia.Games.Domain.Events;

public class NightActionsAppliedEvent : DomainEvent
{
    public List<AppliedAction> AppliedActions { get; }

    public NightActionsAppliedEvent(List<AppliedAction> appliedActions)
    {
        AppliedActions = appliedActions;
    }
}