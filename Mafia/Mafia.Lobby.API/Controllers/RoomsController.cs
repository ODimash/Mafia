using Mafia.Lobby.API.RequestModels;
using Mafia.Lobby.Application.Handlers.RoomHandlers.ChangePrivacy;
using Mafia.Lobby.Application.Handlers.RoomHandlers.CreateRoom;
using Mafia.Lobby.Application.Handlers.RoomHandlers.KickUser;
using Mafia.Lobby.Application.Handlers.RoomHandlers.LeaveRoom;
using Mafia.Lobby.Application.Handlers.RoomHandlers.StartGame;
using Mafia.Lobby.Application.Handlers.RoomHandlers.UpdateSettings;
using Mafia.Shared.API.Models;
using Mafia.Shared.Contracts.DTOs.Lobby;
using MediatR;
using Mefia.Shared.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mafia.Lobby.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoomsController : ControllerBase
{
	
	private readonly IMediator  _mediator;
	
	public RoomsController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost("Create")]
	public async Task<ResponseModel<Guid>> CreateRoom([FromBody] CreateRoomRequest request)
	{
		var command = request.ToCommand(User.GetUserId());
		var result = await _mediator.Send(command);
		return result.ToResponse();
	}

	[HttpPost("Join")]
	public async Task<ResponseModel> JoinRoom([FromBody] JoinRoomRequest request)
	{
		var command = request.ToCommand(User.GetUserId());
		var result = await _mediator.Send(command);
		return result.ToResponse();
	}

	[HttpPost("{roomId}/Leave")]
	public async Task<ResponseModel> LeaveRoom(Guid roomId)
	{
		var command = new LeaveRoomCommand() { RoomId = roomId, UserId =  User.GetUserId() };
		var result = await _mediator.Send(command);
		return result.ToResponse();
	}

	[HttpPost("{roomId}/Kick/{userId}")]
	public async Task<ResponseModel> KickFromRoom(Guid roomId, Guid userId)
	{
		var command = new KickUserCommand() { RoomId = roomId, UserId = userId, OwnerId = User.GetUserId() };
		var result = await _mediator.Send(command);
		return result.ToResponse();
	}

	[HttpPost("{roomId}/Start")]
	public async Task<ResponseModel<Guid>> StartGame(Guid roomId)
	{
		var command = new StartGameCommand() { RoomId = roomId, UserId = User.GetUserId() };
		var result = await _mediator.Send(command);
		return result.ToResponse();
	}

	[HttpPut("{roomId}/Settings")]
	public async Task<ResponseModel> UpdateSettings(Guid roomId, [FromBody] RoomSettingsDto settings)
	{
		var command = new UpdateSettingsCommand() { RoomId = roomId, RoomSettings = settings, UserId =  User.GetUserId() };
		var result = await _mediator.Send(command);
		return result.ToResponse();
	}

	[HttpPut("{roomId}/Privacy")]
	public async Task<ResponseModel> ChangePrivacy(Guid roomId, [FromBody] ChangePrivacyRequest request)
	{
		var command = request.ToCommand(roomId, User.GetUserId());
		var result = await _mediator.Send(command);
		return result.ToResponse();
	} 
}
