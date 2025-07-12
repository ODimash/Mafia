
namespace Mafia.Game.Domain.Enums;

public enum Role
{
    Civil,
    Mafia,
    Detective,
    Doctor
}

public static class RoleTypeExtensions
{
    public static SideType GetSide(this Role role) => role switch
    {
        Role.Mafia => SideType.MafiaTeam,
        Role.Civil => SideType.CivilianTeam,
        Role.Doctor => SideType.CivilianTeam,
        Role.Detective => SideType.CivilianTeam,
        _ => SideType.Neutral
    };

    public static bool IsPeaceful(this Role role) =>
        role.GetSide() == SideType.CivilianTeam;

    public static IReadOnlyList<ActionType> GetAvailableActionByRole(this Role role) => role switch
    {
        Role.Civil => [ActionType.Vote],
        Role.Mafia => [ActionType.Vote, ActionType.VotingToKill],
        Role.Detective => [ActionType.Vote, ActionType.Kill, ActionType.CheckIsMafia],
        Role.Doctor => [ActionType.Vote, ActionType.Heal],
        _ => []
    };
}

