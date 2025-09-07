using Mafia.Games.Abstraction.Notifiers;
using Mafia.Shared.Contracts.DTOs;
using Mafia.Shared.Contracts.DTOs.Games;
using Mafia.Shared.Kernel.Enums;
using Mafia.Shared.Infrastructure.Notifiers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Mafia.Games.API.Hubs.Notifiers;

public class GameNotifier : HubNotifier<GameHub>, IGameNotifier
{

	public GameNotifier(IHubContext<GameHub> hubContext, ILogger<HubNotifier<GameHub>> logger) : base(hubContext, logger)
	{
	}
	public Task NotifyDiedPlayers(Guid gameId, List<Guid> playersId, PhaseType phaseType)
	{
		return SendToGroup($"game-{gameId}", "DiedPlayers", new { gameId, playersId, phaseType });
	}
	public Task NotifySheriffAboutCheckingResult(Guid playerId, Guid checkedPlayeerId, bool isMafia)
	{
		return SendToGroup($"player-{playerId}", "CheckingResult", new { checkedPlayeerId, isMafia });
	}

	public Task NotifyNewPhase(Guid gameId, PhaseType prevPhase, PhaseType nextPhase)
	{
		return SendToGroup($"game-{gameId}", "NewPhase", new { prevPhase, nextPhase });
	}
	public Task NotifyGameStarted(Guid gameId)
	{
		return SendToGroup($"game-{gameId}", "GameStarted", new { gameId });
	}
	public Task NotifyGameEnded(Guid gameId, GameResult gameResult)
	{
		return SendToGroup($"game-{gameId}", "GameEnded", new { gameId, gameResult });
	}
}
