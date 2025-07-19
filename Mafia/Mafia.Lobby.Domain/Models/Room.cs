
using FluentResults;
using Mafia.Shared.Kernel;

namespace Mafia.Lobby.Domain.Models;
public class Room : AggregateRoot<Guid>
{
	private readonly List<RoomParticipant> _players = [];
	public IReadOnlyList<RoomParticipant> Players => _players;
	public RoomSettings Settings { get; private set; }
	public Guid OwnerId { get; private set; }
	public RoomPassword Password { get; private set; }
	public RoomCode Code { get; }

	private Room(RoomSettings settings, Guid ownerId, RoomPassword password, RoomCode code)
	{
        Id = Guid.NewGuid();
		Settings = settings;
		OwnerId = ownerId;
		Password = password;
		Code = code;
	}

	public static Result<Room> Create(RoomSettings settings, Guid ownerId, RoomPassword password, RoomCode code)
	{
		if (ownerId == Guid.Empty)
			return Result.Fail("Room cannot be created without owner");

		return new Room(settings, ownerId, password, code);
	}
}
