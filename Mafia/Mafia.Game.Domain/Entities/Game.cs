using FluentResults;
using Mafia.Game.Domain.Enums;
using Mafia.Game.Domain.ValueObjects;
using Mafia.Shared.Kernel;

namespace Mafia.Game.Domain.Entities;

public class Game : Entity<Guid>
{
    public GameSettings Settings { get; private set; }
    public IReadOnlyList<Player> Players { get; private set; }
    public GamePhase CurrentPhase { get; private set; }
    public bool IsFinished { get; private set; }
    public SideType? Winner { get; private set; }

    private Game(
        Guid id,
        GameSettings settings,
        List<Player> players,
        GamePhase currentPhase,
        bool isFinished = false,
        SideType? winner = null)
    {
        Id = id;
        Settings = settings;
        Players = players.AsReadOnly();
        CurrentPhase = currentPhase;
        IsFinished = isFinished;
        Winner = winner;
    }

    public static Result<Game> Create(
        GameSettings settings,
        List<(Guid IdentityId, Role Role)> playerData,
        DateTime firstPhaseEndTime)
    {
        var id = Guid.NewGuid();

        if (playerData.Count != settings.PlayersCount)
            return Result.Fail("Number of players must match the settings' player count");

        var players = new List<Player>();
        foreach (var (identityId, role) in playerData)
        {
            var playerResult = Player.Create(identityId, role);
            if (playerResult.IsFailed)
                return playerResult.ToResult<Game>();
            players.Add(playerResult.Value);
        }

        var roles = playerData.Select(p => p.Role).ToList();
        if (!roles.SequenceEqual(settings.Roles))
            return Result.Fail("Player roles must match the settings' roles");

        var playersForAction = GetPlayersForAction(players, PhaseType.Night);
        var phaseResult = GamePhase.Create(PhaseType.Night, firstPhaseEndTime, playersForAction, new List<RoleAction>());
        if (phaseResult.IsFailed)
            return phaseResult.ToResult<Game>();

        return new Game(id, settings, players, phaseResult.Value);
    }

    public Result PerformAction(Player actor, Player target, ActionType actionType)
    {
        if (IsFinished)
            return Result.Fail("Game is already finished");

        if (!Players.Contains(actor))
            return Result.Fail("Actor is not part of this game");

        if (!Players.Contains(target))
            return Result.Fail("Target is not part of this game");

        if (!CurrentPhase.PlayersForAction.Contains(actor))
            return Result.Fail("Player is not allowed to perform actions in this phase");

        var actionResult = RoleAction.Create(actor, target, actionType);
        if (actionResult.IsFailed)
            return actionResult.ToResult();

        CurrentPhase.PerfectActions.Add(actionResult.Value);
        return Result.Ok();
    }

    public Result ProceedToNextPhase()
    {
        if (IsFinished)
            return Result.Fail("Game is already finished");

        var nextPhaseType = CurrentPhase.Type.GetNextPhase();
        var duration = GetPhaseDuration(nextPhaseType);
        var nextEndTime = DateTime.UtcNow + duration;
        var playersForAction = GetPlayersForAction(Players.ToList(), nextPhaseType);

        var phaseResult = CurrentPhase.ProceedToNextPhase(playersForAction, nextEndTime);
        if (phaseResult.IsFailed)
            return phaseResult;

        ApplyPhaseActions();
        CheckGameEnd();

        return Result.Ok();
    }

    private void ApplyPhaseActions()
    {
        var actions = CurrentPhase.PerfectActions;

        if (CurrentPhase.Type == PhaseType.Night)
        {
            ApplyNightActions(actions);
        }
        else if (CurrentPhase.Type == PhaseType.DayVoting)
        {
            ApplyVotingActions(actions);
        }
    }

    private void ApplyNightActions(List<RoleAction> actions)
    {
        var killActions = actions.Where(a => a.ActionType == ActionType.Kill).ToList();
        var healAction = actions.FirstOrDefault(a => a.ActionType == ActionType.Heal);
        var checkAction = actions.FirstOrDefault(a => a.ActionType == ActionType.CheckIsMafia);

        // Handle kill actions
        var killTarget = killActions.GroupBy(a => a.TargetId)
            .OrderByDescending(g => g.Count())
            .FirstOrDefault()?.Key;

        if (killTarget != null && healAction?.TargetId != killTarget)
        {
            var targetPlayer = Players.FirstOrDefault(p => p.Id == killTarget);
            targetPlayer?.Kill();
        }

        // Handle check action (no state change, just information for detective)
    }

    private Player? ApplyVotingActions(List<RoleAction> actions)
    {
        var voteTarget = actions.Where(a => a.ActionType == ActionType.Vote)
            .GroupBy(a => a.TargetId)
            .OrderByDescending(g => g.Count())
            .FirstOrDefault()?.Key;

        if (voteTarget != null)
        {
            var targetPlayer = Players.FirstOrDefault(p => p.Id == voteTarget);
            targetPlayer?.Kill();
            return targetPlayer;
        }

        return null;
    }

    private void CheckGameEnd()
    {
        var alivePlayers = Players.Where(p => !p.IsKilled).ToList();
        var mafiaCount = alivePlayers.Count(p => p.Role.GetSide() == SideType.MafiaTeam);
        var civilianCount = alivePlayers.Count(p => p.Role.GetSide() == SideType.CivilianTeam);

        if (mafiaCount == 0)
        {
            IsFinished = true;
            Winner = SideType.CivilianTeam;
            UpdateWinners(SideType.CivilianTeam);
        }
        else if (mafiaCount >= civilianCount)
        {
            IsFinished = true;
            Winner = SideType.MafiaTeam;
            UpdateWinners(SideType.MafiaTeam);
        }
    }

    private void UpdateWinners(SideType winningSide)
    {
        foreach (var player in Players)
        {
            player.IsWinner = player.Role.GetSide() == winningSide;
        }
    }

    private static List<Player> GetPlayersForAction(List<Player> players, PhaseType phaseType)
    {
        return phaseType switch
        {
            PhaseType.Night => players.Where(p => !p.IsKilled && p.Role != Role.Civil).ToList(),
            PhaseType.DayDiscussion => players.Where(p => !p.IsKilled).ToList(),
            PhaseType.DayVoting => players.Where(p => !p.IsKilled).ToList(),
            _ => throw new ArgumentOutOfRangeException(nameof(phaseType))
        };
    }

    private TimeSpan GetPhaseDuration(PhaseType phaseType)
    {
        return phaseType switch
        {
            PhaseType.Night => Settings.NightDuration,
            PhaseType.DayDiscussion => Settings.DayDiscussionDuration,
            PhaseType.DayVoting => Settings.VotingDuration,
            _ => throw new ArgumentOutOfRangeException(nameof(phaseType))
        };
    }
}