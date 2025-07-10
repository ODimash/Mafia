
namespace Mafia.Game.Domain.Enums;

public enum RoleType
{
    Civil,
    Mafia,
    Detective,
    Doctor
}

public static class RoleTypeExtensions
{
    public static SideType GetSide(this RoleType role) => role switch
    {
        RoleType.Mafia => SideType.MafiaTeam,
        RoleType.Civil => SideType.CivilianTeam,
        RoleType.Doctor => SideType.CivilianTeam,
        RoleType.Detective => SideType.CivilianTeam,
        _ => SideType.Neutral
    };

    public static bool IsPeaceful(this RoleType role) =>
        role.GetSide() == SideType.CivilianTeam;
}

