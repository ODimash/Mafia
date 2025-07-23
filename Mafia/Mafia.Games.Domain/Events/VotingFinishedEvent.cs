using Mafia.Games.Domain.Models;
using Mafia.Shared.Kernel;

namespace Mafia.Games.Domain.Events;

public class VotingFinishedEvent : DomainEvent
{
    public Player? Victim { get; }

    public VotingFinishedEvent(Player? victim)
    {
        Victim = victim;
    }
}