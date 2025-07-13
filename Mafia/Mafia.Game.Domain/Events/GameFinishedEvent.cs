using Mafia.Game.Domain.Entities;
using Mafia.Game.Domain.Enums;
using Mafia.Shared.Kernel;

namespace Mafia.Game.Domain.Events
{
    public class GameFinishedEvent : DomainEvent
    {
        public List<Player> Winners { get; }
        public SideType WinnerSide { get; }

        public GameFinishedEvent(List<Player> winners, SideType winnerSide)
        {
            Winners = winners;
            WinnerSide = winnerSide;
        }
    }
}
