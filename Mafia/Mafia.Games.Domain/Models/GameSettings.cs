using FluentResults;
using Mafia.Shared.Kernel;
using Mafia.Shared.Kernel.Constants;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Games.Domain.Models;

public class GameSettings : ValueObject
{
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

        if (playersCount < GameSettingConstants.MinPlayersCount)
            return Result.Fail($"Game cannot be created with less than {GameSettingConstants.MinPlayersCount} players");

        if (playersCount > GameSettingConstants.MaxPlayersCount)
            return Result.Fail($"Game cannot be created with more than {GameSettingConstants.MaxPlayersCount} players");

        if (mafiaTeamCount < 1)
            return Result.Fail("Game must have at least one mafia role");

        if (mafiaTeamCount > playersCount / 2)
            return Result.Fail("The number of mafia cannot be more than half of the players");

        if (!IsValidPhaseDuration(dayDiscussionDuration))
            return Result.Fail($"Day discussion duration must be between {GameSettingConstants.MinPhaseDuration:g} and {GameSettingConstants.MaxPhaseDuration:g}");

        if (!IsValidPhaseDuration(nightDuration))
            return Result.Fail($"Night duration must be between {GameSettingConstants.MinPhaseDuration:g} and {GameSettingConstants.MaxPhaseDuration:g}");

        if (!IsValidPhaseDuration(votingDuration))
            return Result.Fail($"Voting duration must be between {GameSettingConstants.MinPhaseDuration:g} and {GameSettingConstants.MaxPhaseDuration:g}");

        return new GameSettings(dayDiscussionDuration, nightDuration, votingDuration, roles);
    }

    private static bool IsValidPhaseDuration(TimeSpan duration) =>
        duration >= GameSettingConstants.MinPhaseDuration && duration <= GameSettingConstants.MaxPhaseDuration;

    public Result<GameSettings> ChangeDayDiscussionDuration(TimeSpan newDuration)
    {
        if (!IsValidPhaseDuration(newDuration))
            return Result.Fail($"Day discussion duration must be between {GameSettingConstants.MinPhaseDuration:g} and {GameSettingConstants.MaxPhaseDuration:g}");

        return new GameSettings(newDuration, NightDuration, VotingDuration, Roles.ToList());
    }

    public Result<GameSettings> ChangeNightDuration(TimeSpan newDuration)
    {
        if (!IsValidPhaseDuration(newDuration))
            return Result.Fail($"Night duration must be between {GameSettingConstants.MinPhaseDuration:g} and {GameSettingConstants.MaxPhaseDuration:g}");

        return new GameSettings(DayDiscussionDuration, newDuration, VotingDuration, Roles.ToList());
    }

    public Result<GameSettings> ChangeVotingDuration(TimeSpan newDuration)
    {
        if (!IsValidPhaseDuration(newDuration))
            return Result.Fail($"Voting duration must be between {GameSettingConstants.MinPhaseDuration:g} and {GameSettingConstants.MaxPhaseDuration:g}");

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
