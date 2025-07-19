using FluentResults;
using Mafia.Games.Domain.ValueObjects;
using Mafia.Shared.Kernel;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Games.Domain.Entities;

public class Player : Entity<Guid>
{
    public Guid IdentityId { get; }
    public Role Role { get; }
    public bool IsKilled { get; private set; }
    public bool IsWinner { get; set; }

    private Player(Guid identityId, Role role, bool isKilled = false, bool isWinner = false)
    {
        Role = role;
        IsKilled = isKilled;
        IdentityId = identityId;
        IsWinner = isWinner;
    }

    public Result Kill()
    {
        if (IsKilled)
            return Result.Fail("The player already dead");

        IsKilled = false;

        return Result.Ok();
    }

    public static Result<Player> Create(Guid identityId, Role role)
    {
        if (identityId == Guid.Empty)
            return Result.Fail("Identity ID can not be empty");

        return new Player(identityId, role);
    }

}
