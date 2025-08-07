using FluentResults;
using Mafia.Lobby.Abstraction.Repositories;
using Mafia.Lobby.Domain.Models;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Lobby.Application.Handlers.RoomHandlers.JoinRoom;

public class JoinRoomHandler : ICommandHandler<JoinRoomCommand, Result>
{
	private readonly IRoomRepository _repository;
	
	public JoinRoomHandler(IRoomRepository repository)
	{
		_repository = repository;
	}

	public async Task<Result> Handle(JoinRoomCommand request, CancellationToken cancellationToken)
	{
		Room? room = null;
		if (request.RoomCode != null)
			room = await _repository.GetRoomByCode(request.RoomCode, cancellationToken);
		else if (request.RoomId != null)
			room = await _repository.GetRoomById(request.RoomId.Value, cancellationToken);
		else
			return Result.Fail("No room identifiers specified");
		
		if (room == null)
			return Result.Fail("No room found");
		
		var result = room.Join(request.UserId, request.Password);
		if (result.IsSuccess)
			await _repository.UpdateRoom(room, cancellationToken);
		
		return result;
	}
}
