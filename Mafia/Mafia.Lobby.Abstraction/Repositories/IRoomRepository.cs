using Mafia.Lobby.Domain.Models;

namespace Mafia.Lobby.Abstraction.Repositories;

public interface IRoomRepository
{
	public Task<bool> RoomCodeExists(string code, CancellationToken token);
	public Task AddRoom(Room room, CancellationToken token);
	public Task<Room?> GetRoomById(Guid id, CancellationToken token);
	public Task<Room?> GetRoomByCode(string code, CancellationToken token);
	public Task UpdateRoom(Room room, CancellationToken token);
}
