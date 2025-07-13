
using Mafia.Game.Domain.ValueObjects;
using Mafia.Shared.Kernel;

namespace Mafia.Game.Domain.Events
{
    public class ActionPerformedEvent : DomainEvent
    {
        public RoleAction PerformedAction { get; set; }

        public ActionPerformedEvent(RoleAction performedAction)
        {
            PerformedAction = performedAction;
        }
    }
}
