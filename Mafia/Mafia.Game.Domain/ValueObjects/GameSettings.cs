using FluentResults;
using Mafia.Game.Domain.Enums;
using Mafia.Shared.Kernel;

namespace Mafia.Game.Domain.ValueObjects;

public class GameSettings : ValueObject
{
    public static readonly int MinPlayersCount = 5;
    public static readonly int MaxPlayersCount = 12;
    public static readonly TimeSpan MaxPhaseDuration = TimeSpan.FromMinutes(5);
    public static readonly TimeSpan MinPhaseDuration = TimeSpan.FromSeconds(30);

    public int PlayersCount => Roles.Count;
    public int MafiaCount => Roles.Count(x => x.GetSide() == SideType.MafiaTeam);

    public TimeSpan DayDiscussionDuration { get; }
    public TimeSpan NightDuration { get; }
    public TimeSpan VotingDuration { get; }

    public IReadOnlyList<Role> Roles { get; }

    private GameSettings(
        TimeSpan dayDiscussionDuration,
        TimeSpan nightDuration,
        TimeSpan votingDuration,
        List<Role> roles)
    {
        DayDiscussionDuration = dayDiscussionDuration;
        NightDuration = nightDuration;
        VotingDuration = votingDuration;
        Roles = roles.AsReadOnly();
    }

    public static Result<GameSettings> Create(
        TimeSpan dayDiscussionDuration,
        TimeSpan nightDuration,
        TimeSpan votingDuration,
        List<Role> roles)
    {
        if (roles is null || roles.Count == 0)
            return Result.Fail("Role list must not be empty");

        int playersCount = roles.Count;
        int mafiaTeamCount = roles.Count(r => r.GetSide() == SideType.MafiaTeam);

        if (playersCount < MinPlayersCount)
            return Result.Fail($"Game cannot be created with less than {MinPlayersCount} players");

        if (playersCount > MaxPlayersCount)
            return Result.Fail($"Game cannot be created with more than {MaxPlayersCount} players");

        if (mafiaTeamCount < 1)
            return Result.Fail("Game must have at least one mafia role");

        if (mafiaTeamCount > playersCount / 2)
            return Result.Fail("The number of mafia cannot be more than half of the players");

        if (!IsValidPhaseDuration(dayDiscussionDuration))
            return Result.Fail($"Day discussion duration must be between {MinPhaseDuration:g} and {MaxPhaseDuration:g}");

        if (!IsValidPhaseDuration(nightDuration))
            return Result.Fail($"Night duration must be between {MinPhaseDuration:g} and {MaxPhaseDuration:g}");

        if (!IsValidPhaseDuration(votingDuration))
            return Result.Fail($"Voting duration must be between {MinPhaseDuration:g} and {MaxPhaseDuration:g}");

        return new GameSettings(dayDiscussionDuration, nightDuration, votingDuration, roles);
    }

    private static bool IsValidPhaseDuration(TimeSpan duration) =>
        duration >= MinPhaseDuration && duration <= MaxPhaseDuration;

    public Result<GameSettings> ChangeDayDiscussionDuration(TimeSpan newDuration)
    {
        if (!IsValidPhaseDuration(newDuration))
            return Result.Fail($"Day discussion duration must be between {MinPhaseDuration:g} and {MaxPhaseDuration:g}");

        return new GameSettings(newDuration, NightDuration, VotingDuration, Roles.ToList());
    }

    public Result<GameSettings> ChangeNightDuration(TimeSpan newDuration)
    {
        if (!IsValidPhaseDuration(newDuration))
            return Result.Fail($"Night duration must be between {MinPhaseDuration:g} and {MaxPhaseDuration:g}");

        return new GameSettings(DayDiscussionDuration, newDuration, VotingDuration, Roles.ToList());
    }

    public Result<GameSettings> ChangeVotingDuration(TimeSpan newDuration)
    {
        if (!IsValidPhaseDuration(newDuration))
            return Result.Fail($"Voting duration must be between {MinPhaseDuration:g} and {MaxPhaseDuration:g}");

        return new GameSettings(DayDiscussionDuration, NightDuration, newDuration, Roles.ToList());
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return DayDiscussionDuration;
        yield return NightDuration;
        yield return VotingDuration;

        foreach (var role in Roles.OrderBy(r => r))
            yield return role;
    }
}
