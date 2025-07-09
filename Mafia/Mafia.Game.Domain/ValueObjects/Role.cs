
using Mafia.Game.Domain.Enums;
using Mafia.Shared.Kernel;

namespace Mafia.Game.Domain.ValueObjects;

public class Role : ValueObject
{
    public RoleType Type { get; }
    public readonly IReadOnlyList<ActionType> ActionTypes;


    public Role(RoleType type)
    {
        Type = type;
        ActionTypes = GetAvailableActionByRole(type);
    }

    private IReadOnlyList<ActionType> GetAvailableActionByRole(RoleType role)
    {
        return role switch
        {
            RoleType.Civil => [ActionType.Vote],
            RoleType.Mafia => [ActionType.Vote, ActionType.Kill],
            RoleType.Detective => [ActionType.Vote, ActionType.Kill, ActionType.CheckIsMafia],
            RoleType.Doctor => [ActionType.Vote, ActionType.Heal],
            _ => []
        };
    }

    public static Role Mafia => new(RoleType.Mafia);
    public static Role Civil => new(RoleType.Civil);
    public static Role Doctor => new(RoleType.Doctor);
    public static Role Detective => new(RoleType.Detective);
    public static Role Viewer => new(RoleType.Viewer);
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Type;
    }
}
