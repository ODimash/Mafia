using Mafia.Games.Domain.Models;
using Mafia.Shared.Kernel;

namespace Mafia.Games.Domain.Events;

public class ActionPerformedEvent : DomainEvent
{
    public PlayerAction PerformedAction { get; set; }

    public ActionPerformedEvent(PlayerAction performedAction)
    {
        PerformedAction = performedAction;
    }
}