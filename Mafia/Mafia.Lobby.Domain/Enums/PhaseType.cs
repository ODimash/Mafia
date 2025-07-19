
namespace Mafia.Game.Domain.Enums;

public enum PhaseType
{
    Night,
    DayDiscussion,
    DayVoting,
}

public static class PhaseTypeExtensions
{
    public static PhaseType GetNextPhase(this PhaseType current) => current switch
    {
        PhaseType.Night => PhaseType.DayDiscussion,
        PhaseType.DayDiscussion => PhaseType.DayVoting,
        PhaseType.DayVoting => PhaseType.Night,
        _ => throw new ArgumentOutOfRangeException(nameof(current), $"Unknown phase: {current}")
    };

    public static bool IsDiscussion(this PhaseType current)
    {
        return current == PhaseType.Night || current == PhaseType.DayDiscussion;
    }
}
