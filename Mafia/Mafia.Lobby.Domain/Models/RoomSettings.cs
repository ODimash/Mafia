using FluentResults;
using Mafia.Shared.Kernel;
using Mafia.Shared.Kernel.Constants;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Lobby.Domain.Models;

public class RoomSettings : ValueObject
{

    public TimeSpan DayDiscussionDuration { get; }
    public TimeSpan NightDuration { get; }
    public TimeSpan VotingDuration { get; }
    public int MaxPlayersCount { get; private set; }
    public int MinPlayersCount { get; private set; }

    public IReadOnlyList<Role> EnabledRoles { get; }


    private RoomSettings(
        TimeSpan dayDiscussionDuration,
        TimeSpan nightDuration, 
        TimeSpan votingDuration, 
        IReadOnlyList<Role> enabledRoles, 
        int maxPlayersCount, 
        int minPlayersCount)
    {
        DayDiscussionDuration = dayDiscussionDuration;
        NightDuration = nightDuration;
        VotingDuration = votingDuration;
        EnabledRoles = enabledRoles;
        MaxPlayersCount = maxPlayersCount;
        MinPlayersCount = minPlayersCount;
    }


    public static Result<RoomSettings> Create(
        TimeSpan dayDiscussionDuration, 
        TimeSpan nightDuration, 
        TimeSpan votingDuration, 
        IReadOnlyList<Role> enabledRoles,
        int maxPlayersCount,
        int minPlayersCount)
    {
        if (minPlayersCount < GameSettingConstants.MinPlayersCount)
            return Result.Fail($"Game cannot be created with less than {GameSettingConstants.MinPlayersCount} players");

        if (maxPlayersCount > GameSettingConstants.MaxPlayersCount)
            return Result.Fail($"Game cannot be created with more than {GameSettingConstants.MaxPlayersCount} players");

        if (!IsValidPhaseDuration(dayDiscussionDuration))
            return Result.Fail($"Day discussion duration must be between {GameSettingConstants.MinPhaseDuration:g} and {GameSettingConstants.MaxPhaseDuration:g}");

        if (!IsValidPhaseDuration(nightDuration))
            return Result.Fail($"Night duration must be between {GameSettingConstants.MinPhaseDuration:g} and {GameSettingConstants.MaxPhaseDuration:g}");

        if (!IsValidPhaseDuration(votingDuration))
            return Result.Fail($"Voting duration must be between {GameSettingConstants.MinPhaseDuration:g} and {GameSettingConstants.MaxPhaseDuration:g}");

        return new RoomSettings(dayDiscussionDuration, nightDuration, votingDuration, enabledRoles, maxPlayersCount, minPlayersCount);
    }


    private static bool IsValidPhaseDuration(TimeSpan duration) =>
        duration >= GameSettingConstants.MinPhaseDuration && duration <= GameSettingConstants.MaxPhaseDuration;

        
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return DayDiscussionDuration;
        yield return NightDuration;
        yield return VotingDuration;
        yield return MaxPlayersCount;
        yield return MinPlayersCount;
        foreach (var enabledRole in EnabledRoles)
            yield return enabledRole;
    }
}