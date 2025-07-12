
namespace Mafia.Game.Domain.Enums;

public enum ActionType
{
    Kill,
    CheckIsMafia,
    Heal,
    Vote,
    VotingToKill,
    Block,
}

public static class ActionTypeExtensions
{
    public static PhaseType GetPhase(this ActionType actionType)
    {
        return actionType switch
        {
            ActionType.Kill => PhaseType.Night,
            ActionType.Heal => PhaseType.Night,
            ActionType.Block => PhaseType.Night,
            ActionType.CheckIsMafia => PhaseType.Night,
            ActionType.Vote => PhaseType.DayVoting,
            ActionType.VotingToKill => PhaseType.Night,

            _ => throw new ArgumentOutOfRangeException(nameof(actionType), actionType, "Unknown action type")
        };
    }
}