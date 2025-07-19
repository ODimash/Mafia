using FluentResults;
using Mafia.Game.Domain.Events;
using Mafia.Game.Domain.ValueObjects;
using Mafia.Shared.Kernel;
using Mafia.Shared.Kernel.Enums;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace Mafia.Game.Domain.Entities
{
    public class GameSession : AggregateRoot<Guid>
    {
        public GameSettings Settings { get; private set; }
        public IReadOnlyList<Player> Players { get; private set; }
        public GamePhase CurrentPhase { get; private set; }
        public bool IsFinished { get; private set; }
        public SideType? Winner { get; private set; }

        private GameSession(
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

        public static Result<GameSession> Create(
            GameSettings settings,
            List<(Guid IdentityId, Role Role)> playerData)
        {
            var id = Guid.NewGuid();

            if (playerData.Count != settings.PlayersCount)
                return Result.Fail("Number of players must match the settings player count");

            var players = new List<Player>(playerData.Count);
            foreach (var (identityId, role) in playerData)
            {
                var playerResult = Player.Create(identityId, role);
                if (playerResult.IsFailed)
                    return playerResult.ToResult<GameSession>();
                players.Add(playerResult.Value);
            }

            var roles = playerData.Select(p => p.Role).ToList();
            if (!roles.SequenceEqual(settings.Roles))
                return Result.Fail("Player roles must match the settings' roles");

            var playersForAction = GetPlayersForAction(players, PhaseType.Night);
            var firstPhaseEndTime = DateTime.UtcNow.Add(settings.NightDuration);
            var phaseResult = GamePhase.Create(PhaseType.Night, firstPhaseEndTime, playersForAction);
            if (phaseResult.IsFailed)
                return phaseResult.ToResult<GameSession>();

            return new GameSession(id, settings, players, phaseResult.Value);
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

            if (actionType.GetPhase() != CurrentPhase.Type)
                return Result.Fail("You cannot act now");

            var actionResult = PlayerAction.Create(actor, target, actionType);
            if (actionResult.IsFailed)
                return actionResult.ToResult();

            CurrentPhase.PerfectActions.Add(actionResult.Value);
            AddDomainEvent(new ActionPerformedEvent(actionResult.Value));
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
                return phaseResult.ToResult();

            ApplyPhaseActions();
            CurrentPhase = phaseResult.Value;
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

        private void ApplyNightActions(List<PlayerAction> actions)
        {
            var blockActions = actions.Where(a => a.ActionType == ActionType.Block).ToList();

            List<AppliedAction> appliedActions = new(actions.Count);

            foreach (var action in blockActions)
            {
                actions.RemoveAll(a => a.ActorId == action.TargetId);
            }

            var votesToKill = actions.Where(a => a.ActionType == ActionType.VotingToKill);
            var checkActions = actions.Where(a => a.ActionType == ActionType.CheckIsMafia);
            var killActions = actions.Where(a => a.ActionType == ActionType.Kill).ToList();
            var healActions = actions.Where(a => a.ActionType == ActionType.Heal).ToList();

            foreach (var killAction in killActions)
            {
                var targetToKill = Players.Where(p => p.Id == killAction.TargetId).SingleOrDefault();
                var targetHealed = healActions.Any(a => a.TargetId == killAction.TargetId);
                var actorBlocked = blockActions.Any(a => a.TargetId != killAction.ActorId);

                if (!targetHealed && targetToKill != null)
                {
                    targetToKill.Kill();
                    appliedActions.Add(new(targetToKill.Id, ActionType.Kill));
                }
            }

            foreach (var action in checkActions)
            {
                var targetPlayer = Players.SingleOrDefault(x => x.Id == action.TargetId);
                if (targetPlayer == null)
                    continue;

                var isMafiaTeammate = targetPlayer.Role.GetSide() == SideType.MafiaTeam;
                appliedActions.Add(new(action.TargetId, ActionType.CheckIsMafia, true, isMafiaTeammate.ToString()));
            }

            var mafiaVictimId = votesToKill
                .GroupBy(vk => vk.TargetId)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key;

            var mafiaVictim = Players.Where(p => p.Id == mafiaVictimId).FirstOrDefault();
            var mafiaVictimHealed = healActions.Any(x => x.TargetId == mafiaVictimId);

            if (!mafiaVictimHealed && mafiaVictimId != null)
            {
                var targetPlayer = Players.Where(x => x.Id == mafiaVictimId).FirstOrDefault();
                targetPlayer?.Kill();
                appliedActions.Add(new(mafiaVictimId.Value, ActionType.Kill));
            }

            AddDomainEvent(new NightActionsAppliedEvent(appliedActions));
        }

        private void ApplyVotingActions(List<PlayerAction> actions)
        {
            var votes = actions
                .Where(a => a.ActionType == ActionType.Vote && a.TargetId != Guid.Empty)
                .GroupBy(a => a.TargetId)
                .Select(g => new { TargetId = g.Key, Count = g.Count() })
                .ToList();


            if (votes.Count == 0) return;

            var skips = actions
                .Where(a => a.ActionType == ActionType.Vote && a.TargetId == Guid.Empty)
                .Count();

            var maxVotes = votes.Max(v => v.Count);

            if (skips > maxVotes) 
                return;

            var topCandidates = votes.Where(v => v.Count == maxVotes).ToList();
            if (topCandidates.Count > 1)
                return; // ничья

            var victim = Players.FirstOrDefault(p => p.Id == topCandidates.First().TargetId);
            victim?.Kill();
            AddDomainEvent(new VotingFinishedEvent(victim));
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

            var winners = Players.Where(predicate => predicate.IsWinner).ToList();
            AddDomainEvent(new GameFinishedEvent(winners, winningSide));
        }

        private static List<Player> GetPlayersForAction(List<Player> players, PhaseType phaseType) 
        {
            return players
                .Where(p => !p.IsKilled && p.Role.GetAvailableActionByRole()
                    .Any(a => a.GetPhase() == phaseType))
                .ToList();
        }

        private TimeSpan GetPhaseDuration(PhaseType phaseType) => phaseType switch
        {
            PhaseType.Night => Settings.NightDuration,
            PhaseType.DayDiscussion => Settings.DayDiscussionDuration,
            PhaseType.DayVoting => Settings.VotingDuration,
            _ => throw new ArgumentOutOfRangeException(nameof(phaseType))
        };
    }
}