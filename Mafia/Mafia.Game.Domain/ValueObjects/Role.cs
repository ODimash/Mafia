
using Mafia.Game.Domain.Enums;
using Mafia.Shared.Kernel;

namespace Mafia.Game.Domain.ValueObjects;

public class Role : ValueObject
{
    public RoleType Type{ get; }
    public PhaseType ActivePhase { get; }
    public List<RoleAction> Actions { get; }


    private Role(RoleType type)
    {
        Type = type;
    }

    public static Role Mafia => new(RoleType.Mafia);
    public static Role Civil => new(RoleType.Civil);
    public static Role Doctor => new(RoleType.Doctor);
    public static Role Detective => new(RoleType.Detective);
    public static Role Viewer => new(RoleType.Viewer);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}
