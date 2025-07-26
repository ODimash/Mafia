using Mafia.Games.Domain.Models;
using Mafia.Shared.Kernel;

namespace Mafia.Games.Domain.Events;

public class ActionPerformedDomainEvent : DomainEvent
{
    public PlayerAction PerformedAction { get; set; }

    public ActionPerformedDomainEvent(PlayerAction performedAction)
    {
        PerformedAction = performedAction;
    }
}