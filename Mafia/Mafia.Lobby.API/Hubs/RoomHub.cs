using Mafia.Lobby.Application.EventHandlers.OnPlayerConnectionLost;
using Mafia.Lobby.Application.Handlers.RoomHandlers.JoinRoom;
using MediatR;
using Mefia.Shared.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Text.RegularExpressions;

namespace Mafia.Lobby.API.Hubs;

using Microsoft.AspNetCore.SignalR;

public class RoomHub : Hub
{
    private readonly ILogger<RoomHub> _logger;
    private readonly IMediator _mediator;
	
    public RoomHub(ILogger<RoomHub> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async override Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        if (!Guid.TryParse(httpContext?.Request.Query["roomId"], out var roomId))
            return;

        await Groups.AddToGroupAsync(Context.ConnectionId, $"room-{roomId}");
        await Groups.AddToGroupAsync(Context.ConnectionId, $"player-{Context.User.GetUserId()}");

        await base.OnConnectedAsync();
    }

    public async override Task OnDisconnectedAsync(Exception? exception)
    {
        if (exception != null) 
            _logger.LogError(exception, exception.Message);
        
        var userId = Context.User.GetUserId();
        if (userId != default)
            await _mediator.Publish(new UserConnectionLostEvent(Context.User.GetUserId()));
        
        await base.OnDisconnectedAsync(exception);
    }
}
