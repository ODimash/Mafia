
using Mafia.Game.Domain.Entities;
using Mafia.Shared.Kernel;

namespace Mafia.Game.Domain.Events
{
    public class VotingFinishedEvent : DomainEvent
    {
        public Player? Victim { get; }

        public VotingFinishedEvent(Player? victim)
        {
            Victim = victim;
        }
    }
}
