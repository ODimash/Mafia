using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Mafia.Shared.Infrastructure.Notifiers;

public abstract class HubNotifier<T> where T : Hub
{
	private readonly IHubContext<T> _hubContext;
	private readonly ILogger<HubNotifier<T>> _logger;

	protected HubNotifier(IHubContext<T> hubContext, ILogger<HubNotifier<T>> logger)
	{
		_hubContext = hubContext;
		_logger = logger;
	}

	async protected Task SendToGroup(string groupName, string message, object arg)
	{
		try
		{
			await _hubContext.Clients.Group(groupName).SendAsync(message, arg);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex,
				"Failed while sending signal {Signal} " +
				"to group {GroupName} with arguments {Args}", groupName, message, arg);
		}
	}

	async protected Task SendToUser(string connectionId, string message, object arg)
	{
		try
		{
			await _hubContext.Clients.Client(connectionId).SendAsync(message, arg);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex,
				"Failed while sending signal {Signal} to user " +
				"with connection ID {ConnectionId} and arguments {Args}",
				message, connectionId, arg);
		}
	}

	async protected Task SendToAll(string message, object arg)
	{
		try
		{
			await _hubContext.Clients.All.SendAsync(message, arg);
		}
		catch (Exception e)
		{
			_logger.LogError(e,
				"Failed while sending signal {Signal} to all users " +
				"of hub {HubName} with arguments {Args}",
				message, typeof(T).Name, arg);
		}
	}
}
