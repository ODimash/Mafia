using FluentResults;
using Mafia.Games.Domain.Events;
using Mafia.Shared.Kernel;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Games.Domain.Models;

public class Game : AggregateRoot<Guid>
{
	public GameSettings Settings { get; private set; }
	public int Day { get; private set; } = 0;
	public IReadOnlyList<Player> Players { get; private set; }
	public GamePhase CurrentPhase { get; private set; }
	public bool IsFinished { get; private set; }
	public SideType? WinnerSide { get; private set; }

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
		WinnerSide = winner;
	}

	public static Result<Game> Create(
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
				return playerResult.ToResult<Game>();
			players.Add(playerResult.Value);
		}

		var roles = playerData.Select(p => p.Role).ToList();
		if (!roles.SequenceEqual(settings.Roles))
			return Result.Fail("Player roles must match the settings' roles");

		var playersForAction = GetPlayersForAction(players, PhaseType.Night);
		var firstPhaseEndTime = DateTime.UtcNow.Add(settings.NightDuration);
		var phaseResult = GamePhase.Create(PhaseType.Night, firstPhaseEndTime, playersForAction.Select(p => p.Id).ToList());
		if (phaseResult.IsFailed)
			return phaseResult.ToResult<Game>();
		
		var game = new Game(id, settings, players, phaseResult.Value);
		game.AddDomainEvent(new GameStartedDomainEvent(game.Id));
		return Result.Ok(game);
	}

	internal void PerformAction(PlayerAction action)
	{
		CurrentPhase.AddPerfectAction(action);
		AddDomainEvent(new ActionPerformedDomainEvent(action));
	}

	internal Result KillPlayer(Guid playerId, DeathReason reason, Guid? killerId = null)
	{
		if (IsFinished)
			return Result.Fail("Game is finished");

		var player = Players.SingleOrDefault(p => p.Id == playerId);
		if (player == null)
			return Result.Fail("Player not found");

		if (player.IsKilled)
			return Result.Fail("Player already killed");

		var killResult = player.TryKill();
		if (killResult.IsFailed)
			return killResult;

		AddDomainEvent(new PlayerDiedDomainEvent(playerId, Id, CurrentPhase.Type, reason, killerId));
		return Result.Ok();
	}

	internal Result ProceedToNextPhase(List<Guid> playersIdForAction, DateTime nextEndTime)
	{
		if (IsFinished)
			return Result.Fail("Game is already finished");

		var phaseResult = CurrentPhase.ProceedToNextPhase(playersIdForAction, nextEndTime);
		if (phaseResult.IsFailed)
			return phaseResult.ToResult();

		var oldPhaseType = CurrentPhase.Type;
		CurrentPhase = phaseResult.Value;
		
		if (CurrentPhase.Type == PhaseType.Night)
			Day++;
		
		AddDomainEvent(new GamePhaseChangedDomainEvent(Id, oldPhaseType, CurrentPhase.Type));
		return Result.Ok();
	}

	internal void FinishGame(SideType winningSide)
	{
		IsFinished = true;
		WinnerSide = winningSide;

		foreach (var player in Players)
		{
			player.IsWinner = player.Role.GetSide() == winningSide;
		}

		var winners = Players.Where(predicate => predicate.IsWinner).ToList();
		AddDomainEvent(new GameFinishedDomainEvent(winners, winningSide, Id));
	}

	public static List<Player> GetPlayersForAction(List<Player> players, PhaseType phaseType)
	{
		return players
			.Where(p => !p.IsKilled && p.Role.GetAvailableActionByRole()
				.Any(a => a.GetPhase() == phaseType))
			.ToList();
	}

	internal void CheckIsPlayerMafia(Guid actionActorId, Guid targetPlayerId)
	{
		var isPlayerMafia = Players
			.Where(p => p.Id == targetPlayerId)
			.Select(p => p.Role.GetSide() == SideType.MafiaTeam)
			.FirstOrDefault();

		AddDomainEvent(new CheckedPlayerIsMafiaDomainEvent(actionActorId, targetPlayerId, isPlayerMafia));
	}
}
