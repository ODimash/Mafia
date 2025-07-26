using Mafia.Games.Domain.Models;
using Mafia.Shared.Kernel;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Games.Domain.Events;

public class GameFinishedDomainEvent : DomainEvent
{
    public List<Player> Winners { get; }
    public SideType WinnerSide { get; }
    public Guid GameId { get; }

    public GameFinishedDomainEvent(List<Player> winners, SideType winnerSide, Guid gameId)
    {
        Winners = winners;
        WinnerSide = winnerSide;
        GameId = gameId;
    }
}