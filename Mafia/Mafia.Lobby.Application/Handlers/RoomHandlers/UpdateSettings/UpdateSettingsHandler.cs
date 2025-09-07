using FluentResults;
using Mafia.Lobby.Abstraction.Repositories;
using Mafia.Lobby.Domain.Models;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Lobby.Application.Handlers.RoomHandlers.UpdateSettings;

public class UpdateSettingsHandler : ICommandHandler<UpdateSettingsCommand, Result>
{
	private readonly IRoomRepository  _repository;
	
	public UpdateSettingsHandler(IRoomRepository repository)
	{
		_repository = repository;
	}

	public async Task<Result> Handle(UpdateSettingsCommand request, CancellationToken cancellationToken)
	{
		var room = await _repository.GetRoomById(request.RoomId, cancellationToken);
		if (room == null)
			return Result.Fail("Room not found");
		
		var settings = RoomSettings.Create(
			TimeSpan.FromSeconds(request.RoomSettings.DayDiscussionDurationInSeconds), 
			TimeSpan.FromSeconds(request.RoomSettings.NightDurationInSeconds), 
			TimeSpan.FromSeconds(request.RoomSettings.VotingDurationInSeconds),
			request.RoomSettings.EnabledRoles,
			request.RoomSettings.MaxPlayersCount,
			request.RoomSettings.MinPlayersCount);

		if (settings.IsFailed)
			return settings.ToResult();
		
		room.UpdateSettings(settings.Value);
		await _repository.UpdateRoom(room, cancellationToken);
		return Result.Ok();
	}
}
