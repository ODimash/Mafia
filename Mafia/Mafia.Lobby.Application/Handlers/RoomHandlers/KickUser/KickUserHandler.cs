using FluentResults;
using Mafia.Lobby.Abstraction.Repositories;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Lobby.Application.Handlers.RoomHandlers.KickUser;

public class KickUserHandler : ICommandHandler<KickUserCommand, Result>
{
	private readonly IRoomRepository _repository;
	
	public KickUserHandler(IRoomRepository repository)
	{
		_repository = repository;
	}

	public async Task<Result> Handle(KickUserCommand request, CancellationToken cancellationToken)
	{
		var room = await _repository.GetRoomById(request.RoomId, cancellationToken);
		if (room == null)
			return Result.Fail("Room not found");
		
		if (room.OwnerId != request.OwnerId)
			return Result.Fail("Only the owner of the room can kick");
		
		var result = room.Kick(request.UserId);
		if (result.IsSuccess)
			await _repository.UpdateRoom(room, cancellationToken);
		
		return result;
	}
}
