using Mafia.Games.Contracts.DTOs;
using Mafia.Games.Domain.Models;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Games.Abstraction.Notifiers;

public interface IGameNotifier
{
	Task NotifyDiedPlayers(Guid gameId, List<Guid> playersId, PhaseType phaseType);
	Task NotifySheriffAboutCheckingResult(Guid playerId, Guid checkedPlayeerId, bool isMafia);
	Task NotifyNewPhase(Guid gameId, PhaseType prevPhase, PhaseType nextPhase);
	Task NotifyGameStarted(Guid gameId);
	Task NotifyGameEnded(Guid gameId, GameResult gameResult);
}
