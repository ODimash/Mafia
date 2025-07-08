
using Mafia.Game.Domain.ValueObjects;
using Mafia.Shared.Kernel;

namespace Mafia.Game.Domain.Entities;

public class Player : Entity<Guid>
{
    public Role Role { get; }

    public bool IsKilled { get; }
}
