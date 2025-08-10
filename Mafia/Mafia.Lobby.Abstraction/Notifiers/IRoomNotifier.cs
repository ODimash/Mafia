using Mafia.Lobby.Domain.DomainEvents;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Lobby.Abstraction.Notifiers;

public interface IRoomNotifier
{
	Task NotifyNewPlayer(Guid roomId, Guid userId, Guid participantId);
	Task NotifyLeavedPlayer(Guid roomId, Guid userId);
	Task NotifyKickedPlayer(Guid roomId, Guid identityId);

	Task NotifyGameStartTime(Guid roomId, DateTime startTime);
	Task NotifyGameStartTimeCancelled(Guid roomId);
	Task NotifyStartedGame(Guid userId, Role role);
}
