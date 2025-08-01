using Mafia.Games.Abstraction.Notifiers;
using Mafia.Games.Contracts.DTOs;
using Mafia.Shared.Kernel.Enums;
using Microsoft.AspNetCore.SignalR;

namespace Mafia.Games.API.Hubs.Notifiers;

public class GameNotifier : IGameNotifier
{
	private readonly IHubContext<GameHub> _hubContext;

	public GameNotifier(IHubContext<GameHub> hubContext)
	{
		_hubContext = hubContext;
	}

	public Task NotifyDiedPlayers(Guid gameId, List<Guid> playersId, PhaseType phaseType)
	{
		return _hubContext.Clients.Group(gameId.ToString()).SendAsync("DiedPlayers", playersId, phaseType);
	}
	public Task NotifySheriffAboutCheckingResult(Guid playerId, Guid checkedPlayeerId, bool isMafia)
	{
		return _hubContext.Clients.Group(playerId.ToString()).SendAsync("CheckingResult", checkedPlayeerId, isMafia);
	}

	public Task NotifyNewPhase(Guid gameId, PhaseType prevPhase, PhaseType nextPhase)
	{
		return _hubContext.Clients.Group(gameId.ToString()).SendAsync("NewPhase", prevPhase, nextPhase);
	}
	public Task NotifyGameStarted(Guid gameId)
	{
		return _hubContext.Clients.Group(gameId.ToString()).SendAsync("GameStarted");
	}
	public Task NotifyGameEnded(Guid gameId, GameResult gameResult)
	{
		return _hubContext.Clients.Group(gameId.ToString()).SendAsync("GameEnded", gameResult);
	}
}
