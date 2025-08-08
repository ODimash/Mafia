using FluentResults;
using Mafia.Games.Contracts.DTOs;
using Mafia.Games.Domain.Models;
using Mafia.Lobby.Abstraction.Repositories;
using Mafia.Lobby.Domain.Models;
using Mafia.Lobby.Domain.Services;
using Mafia.Shared.Contracts.Messaging;
using Mafia.Shared.Kernel.Constants;
using Mafia.Shared.Kernel.Enums;
using MediatR;

namespace Mafia.Lobby.Application.Handlers.RoomHandlers.StartGame;

public class StartGameHandler : ICommandHandler<StartGameCommand, Result<Guid>>
{
	private readonly IRoomRepository _roomRepository;
	private readonly IMediator  _mediator;
	private readonly IGameRolesService _gameRolesService;
	

	public StartGameHandler(IRoomRepository roomRepository, IMediator mediator, IGameRolesService gameRolesService)
	{
		_roomRepository = roomRepository;
		_mediator = mediator;
		_gameRolesService = gameRolesService;
	}

	public async Task<Result<Guid>> Handle(StartGameCommand request, CancellationToken cancellationToken)
	{
		var room = await _roomRepository.GetRoomById(request.RoomId, cancellationToken);
		if (room == null)
			return Result.Fail("Room not found");

		if (room.OwnerId != request.UserId)
			return Result.Fail("Only owner can start a game");
		
		var roles = _gameRolesService.GenerateRolesOfEnabledRoles(room.Settings.EnabledRoles, room.Players.Count);
		if (roles.IsFailed)
			return roles.ToResult();
		
		var gameSettings = GenerateGameSettings(room.Settings, roles.Value);
				
		var command = new Mafia.Games.Contracts.Commands.StartGameCommand(gameSettings, room.Players.Select(p => p.Id).ToList());
		return await _mediator.Send(command, cancellationToken);
	}
	private GameSettingsDto GenerateGameSettings(RoomSettings roomSettings, List<Role> roles)
	{
		return new GameSettingsDto
		{
			DayDiscussionDuration = roomSettings.DayDiscussionDuration,
			NightDuration = roomSettings.NightDuration,
			VoteDuration = roomSettings.VotingDuration,
			Roles = roles
		};
	}
}
