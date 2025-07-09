using FluentResults;
using Mafia.Game.Domain.Enums;
using Mafia.Game.Domain.ValueObjects;
using Mafia.Shared.Kernel;

namespace Mafia.Game.Domain.Entities;

public class Player : Entity<Guid>
{
    public Guid IdentityId { get; }
    public Role Role { get; }
    public bool IsKilled { get; }
    public RoleAction? LastAction { get; }
    public bool IsWinner { get; }

    private Player(Guid identityId, Role role, RoleAction? lastAction = null, bool isKilled = false, bool isWinner = false)
    {
        Role = role;
        IsKilled = isKilled;
        LastAction = lastAction;
        IdentityId = identityId;
        IsWinner = isWinner;
    }

    public static Result<Player> Create(Guid identityId, RoleType role)
    {
        if (identityId == Guid.Empty)
            return Result.Fail("Identity ID can not be empty");

        return new Player(identityId, new Role(role));
    }

}
