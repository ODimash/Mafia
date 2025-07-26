using FluentResults;
using Mafia.Games.Domain.Models;
using Mafia.Games.Domain.Services.Interfaces;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Games.Domain.Services;

public class GameActionService : IGameActionService
{
	public Result PerformAction(Game game, Guid actorId, Guid targetId, ActionType actionType)
	{
		if (game.IsFinished)
			return Result.Fail("Game is already finished");

		if (!game.Players.Any(p => p.Id == actorId))
			return Result.Fail("Actor is not part of this game");

		if (!game.Players.Any(p => p.Id == targetId))
			return Result.Fail("Target is not part of this game");

		if (!game.CurrentPhase.PlayersForAction.Any((pId => pId == actorId)))
			return Result.Fail("Player is not allowed to perform actions in this phase");

		if (actionType.GetPhase() != game.CurrentPhase.Type)
			return Result.Fail("You cannot act now");

		var actionResult = PlayerAction.Create(actorId, targetId, actionType);
		if (actionResult.IsFailed)
			return actionResult.ToResult();

		game.PerformAction(actionResult.Value);

		return Result.Ok();
	}


	public List<Player> GetPlayersForActionAtNextPhase(Game game)
	{
		return game.Players
			.Where(p => !p.IsKilled && p.Role.GetAvailableActionByRole()
				.Any(a => a.GetPhase() == game.CurrentPhase.Type.GetNextPhase()))
			.ToList();
	}

	public void ApplyPhaseActions(Game game)
	{
		if (game.CurrentPhase.Type == PhaseType.Night)
		{
			ApplyNightActions(game);
		}
		else if (game.CurrentPhase.Type == PhaseType.DayVoting)
		{
			ApplyVotingActions(game);
		}
	}

	private void ApplyNightActions(Game game)
	{
		var actions = game.CurrentPhase.PerfectActions.ToList();
		var blockActions = actions.Where(a => a.ActionType == ActionType.Block).ToList();

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
			var targetToKill = game.Players.Where(p => p.Id == killAction.TargetId).SingleOrDefault();
			var targetHealed = healActions.Any(a => a.TargetId == killAction.TargetId);

			if (!targetHealed && targetToKill != null)
			{
				game.KillPlayer(targetToKill.Id, DeathReason.AtNight);
			}
		}

		foreach (var action in checkActions)
		{
			var targetPlayer = game.Players.SingleOrDefault(x => x.Id == action.TargetId);
			if (targetPlayer == null)
				continue;

			game.CheckIsPlayerMafia(action.ActorId, targetPlayer.Id);
		}

		KillMafiaVictim(game, votesToKill, healActions);
	}
	private static void KillMafiaVictim(Game game, IEnumerable<PlayerAction> votesToKill, List<PlayerAction> healActions)
	{
		var mafiaVictimId = votesToKill
			.GroupBy(vk => vk.TargetId)
			.OrderByDescending(g => g.Count())
			.FirstOrDefault()?.Key;

		if (mafiaVictimId == null)
			return;

		var isHealed = healActions.Any(x => x.TargetId == mafiaVictimId);
		if (isHealed)
			return;

		game.KillPlayer(mafiaVictimId.Value, DeathReason.AtNight);
	}

	private void ApplyVotingActions(Game game)
	{
		var actions = game.CurrentPhase.PerfectActions.ToList();

		var votes = actions
			.Where(a => a.ActionType == ActionType.Vote && a.TargetId != Guid.Empty)
			.GroupBy(a => a.TargetId)
			.Select(g => new { TargetId = g.Key, Count = g.Count() })
			.ToList();

		if (votes.Count == 0)
			return;

		var skips = actions
			.Where(a => a.ActionType == ActionType.Vote && a.TargetId == Guid.Empty)
			.Count();

		var maxVotes = votes.Max(v => v.Count);

		if (skips > maxVotes)
			return;

		var topCandidates = votes.Where(v => v.Count == maxVotes).ToList();
		if (topCandidates.Count > 1)
			return; // ничья

		Guid victim = topCandidates.First().TargetId;
		game.KillPlayer(victim, DeathReason.Voting);
	}

}
