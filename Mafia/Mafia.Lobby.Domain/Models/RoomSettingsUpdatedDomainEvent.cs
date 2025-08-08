using Mafia.Shared.Kernel;

namespace Mafia.Lobby.Domain.Models;

public class RoomSettingsUpdatedDomainEvent : DomainEvent
{
	public RoomSettingsUpdatedDomainEvent(Guid roomId, RoomSettings updatedRoomSettings)
	{
		RoomId = roomId;
		UpdatedRoomSettings = updatedRoomSettings;
	}
	public Guid RoomId { get; }
	public RoomSettings UpdatedRoomSettings { get; }
}
