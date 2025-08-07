using FluentResults;
using Mafia.Lobby.Abstraction.Repositories;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Lobby.Application.Handlers.RoomHandlers.LeaveRoom;

public class LeaveRoomHandlers : ICommandHandler<LeaveRoomCommand, Result>
{
	private readonly IRoomRepository _roomRepository;
	
	public LeaveRoomHandlers(IRoomRepository roomRepository)
	{
		_roomRepository = roomRepository;
	}

	public async Task<Result> Handle(LeaveRoomCommand request, CancellationToken cancellationToken)
	{
		var room = await _roomRepository.GetRoomById(request.RoomId, cancellationToken);
		if (room == null)
			return Result.Fail("No room found");
		
		var result = room.Leave(request.UserId);
		if (result.IsSuccess)
			await _roomRepository.UpdateRoom(room, cancellationToken);
		
		return result;
	}
}
