namespace Mafia.Lobby.Abstraction.Notifiers;

public interface ILobbyNotifier
{
	Task NotifyNewRoom(Guid roomId);
	Task NotifyChangedPrivacy(Guid roomId, bool isPrivate);
}
