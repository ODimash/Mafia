
using Mafia.Game.Domain.ValueObjects;
using Mafia.Shared.Kernel;

namespace Mafia.Game.Domain.Events
{
    public class ActionPerformedEvent : DomainEvent
    {
        public PlayerAction PerformedAction { get; set; }

        public ActionPerformedEvent(PlayerAction performedAction)
        {
            PerformedAction = performedAction;
        }
    }
}
