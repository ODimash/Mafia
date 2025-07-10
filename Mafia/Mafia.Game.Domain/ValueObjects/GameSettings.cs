
using FluentResults;
using Mafia.Game.Domain.Enums;
using Mafia.Shared.Kernel;

namespace Mafia.Game.Domain.ValueObjects;

public class GameSettings : ValueObject
{
    public static readonly int MinPlayersCount = 5;
    public static readonly int MaxPlayersCount = 12;

    public int PlayersCount { get; private set; }
    public int MafiaCount { get; private set; }
    public TimeSpan DayDiscussionDuration { get; private set; }
    public TimeSpan NightDiscussionDuration { get; private set; }

    public List<RoleType> IncludedRoles { get; private set; } = [];

    private GameSettings(int playersCount, int mafiaCount, List<RoleType> includedRoles)
    {
        PlayersCount = playersCount;
        MafiaCount = mafiaCount;
        IncludedRoles = includedRoles;
    }

    public static Result<GameSettings> Create(int playersCount, int mafiaCount, List<RoleType> includedRoles)
    {
        if (playersCount < MinPlayersCount)
            return Result.Fail($"Game cannot be created with less than {MinPlayersCount} player");

        if (playersCount > MaxPlayersCount)
            return Result.Fail($"Game cannot be created with more than {MaxPlayersCount} player");

        if (mafiaCount < 1)
            return Result.Fail("Game cannot be created without mafia");

        if (mafiaCount > playersCount / 2)
            return Result.Fail("The number of mafia cannot be more than half of the peaceful team");

        return new GameSettings(playersCount, mafiaCount, includedRoles);
    }


    protected override IEnumerable<object?> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}
