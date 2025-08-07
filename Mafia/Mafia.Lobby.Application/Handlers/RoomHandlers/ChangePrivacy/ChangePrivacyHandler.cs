using FluentResults;
using Mafia.Lobby.Abstraction.Repositories;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Lobby.Application.Handlers.RoomHandlers.ChangePrivacy;

public class ChangePrivacyHandler : ICommandHandler<ChangePrivacyCommand, Result>
{
	private readonly IRoomRepository _repository;
	
	public ChangePrivacyHandler(IRoomRepository repository)
	{
		_repository = repository;
	}

	public async Task<Result> Handle(ChangePrivacyCommand request, CancellationToken cancellationToken)
	{
		var room = await _repository.GetRoomById(request.RoomId, cancellationToken);
		if (room == null)
			return Result.Fail("Room not found");
		
		if (room.OwnerId != request.UserId)
			return Result.Fail("Only  the owner of the room can be changed");
		
		var result = room.ChangePrivacy(request.IsPrivate);
		if (result.IsSuccess)
			await _repository.UpdateRoom(room, cancellationToken);
		
		return result;
	}
}
