
namespace Mafia.Game.Domain.Enums;

public enum PhaseType
{
    NightDiscussion,
    NightVoting,
    DayDiscussion,
    DayVoting,
}

public static class PhaseTypeExtensions
{
    public static PhaseType GetNextPhase(this PhaseType current) => current switch
    {
        PhaseType.NightDiscussion => PhaseType.NightVoting,
        PhaseType.NightVoting => PhaseType.DayDiscussion,
        PhaseType.DayDiscussion => PhaseType.DayVoting,
        PhaseType.DayVoting => PhaseType.NightDiscussion,
        _ => throw new ArgumentOutOfRangeException(nameof(current), $"Unknown phase: {current}")
    };

    public static bool IsDiscussion(this PhaseType current)
    {
        return current == PhaseType.NightDiscussion || current == PhaseType.DayDiscussion;
    }
}
