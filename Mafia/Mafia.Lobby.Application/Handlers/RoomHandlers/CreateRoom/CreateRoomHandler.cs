using AutoMapper;
using FluentResults;
using Mafia.Lobby.Abstraction.Repositories;
using Mafia.Lobby.Domain.Models;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Lobby.Application.Handlers.RoomHandlers.CreateRoom;

public class CreateRoomHandler : ICommandHandler<CreateRoomCommand, Result<Guid>>
{
	private readonly IRoomRepository _repository;
	private readonly IMapper _mapper;
	private readonly Random _random =  new Random();
	
	public CreateRoomHandler(IRoomRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<Result<Guid>> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
	{
		var settingsResult = RoomSettings.Create(
			request.Settings.DayDiscussionDuration, 
			request.Settings.NightDuration, 
			request.Settings.VotingDuration,
			request.Settings.EnabledRoles,
			request.Settings.MaxPlayersCount,
			request.Settings.MinPlayersCount);
		
		if  (settingsResult.IsFailed)
			return settingsResult.ToResult();

		string roomCodeAsStr;
		do roomCodeAsStr = GenerateRoomCode();
		while(await _repository.RoomCodeExists(roomCodeAsStr, cancellationToken));
		
		var roomCode = RoomCode.Create(roomCodeAsStr);
		if (roomCode.IsFailed)
			throw new Exception(roomCode.Errors.Select(error => error.Message).FirstOrDefault());
		
		var roomName = RoomName.Create(roomCodeAsStr);
		
		var room = Room.Create(
			settingsResult.Value, 
			request.OwnerId, 
			roomCode.Value, 
			roomName.Value, 
			request.IsPrivate);
		
		if (room.IsFailed)
			return room.ToResult();

		await _repository.AddRoom(room.Value, cancellationToken);

		return room.Value.Id;
	}

	private string GenerateRoomCode()
	{
		const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
		var random = new Random();
		return new string(Enumerable.Range(0, 6)
			.Select(_ => chars[random.Next(chars.Length)]).ToArray());
	}

}
