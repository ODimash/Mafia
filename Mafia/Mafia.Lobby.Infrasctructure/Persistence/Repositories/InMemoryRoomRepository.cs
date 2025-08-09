using Mafia.Lobby.Abstraction.Repositories;
using Mafia.Lobby.Domain.Models;
using System.Collections.Concurrent;

namespace Mafia.Lobby.Infrasctructure.Persistence.Repositories;

public class InMemoryRoomRepository : IRoomRepository
{
	private readonly ConcurrentDictionary<Guid, Room> _rooms = [];

	public Task<bool> RoomCodeExists(string code, CancellationToken token)
	{
		return Task.FromResult(
			_rooms.Values.Any(room => room.Code == code)
		);
	}
	public Task AddRoom(Room room, CancellationToken token)
	{
		if (!_rooms.TryAdd(room.Id, room))
		{
			throw new InvalidOperationException("Room already exists");
		}

		return Task.CompletedTask;
	}
	public Task<Room?> GetRoomById(Guid id, CancellationToken token)
	{
		return Task.FromResult(_rooms.TryGetValue(id, out var room) ? room : null);
	}
	public Task<Room?> GetRoomByCode(string code, CancellationToken token)
	{
		return  Task.FromResult(_rooms.Values.FirstOrDefault(room => room.Code == code));
	}
	public Task UpdateRoom(Room room, CancellationToken token)
	{
		_rooms.AddOrUpdate(room.Id, room, (_, _) => room);
		return Task.CompletedTask;
	}
}
