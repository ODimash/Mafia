using Mafia.Lobby.Domain.DomainEvents;

namespace Mafia.Lobby.Abstraction.Notifiers;

public interface IRoomNotifier
{
	Task NotifyNewPlayer(Guid roomId, Guid userId, Guid participantId);
	Task NotifyLeavedPlayer(Guid roomId, Guid userId);
	Task NotifyKickedPlayer(Guid roomId, Guid identityId);
	Task NotifyNewRoom(Guid roomId);
	Task NotifyChangedPrivacy(Guid roomId, bool isPrivate);
	Task NotifyGameStartTime(Guid roomId, DateTime startTime);
	Task NotifyGameStartTimeCancelled(Guid roomId);
}
