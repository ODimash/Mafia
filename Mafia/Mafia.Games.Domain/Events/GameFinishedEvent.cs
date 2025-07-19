using Mafia.Games.Domain.Entities;
using Mafia.Shared.Kernel;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Games.Domain.Events;

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