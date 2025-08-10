using Mafia.Shared.Contracts.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Mefia.Shared.Infrastructure.Messaging;

public class MediatorEventBus : IEventBus
{
	private readonly IMediator _mediator;
	private readonly ILogger<MediatorEventBus> _logger;
	
	public MediatorEventBus(IMediator mediator, ILogger<MediatorEventBus> logger)
	{
		_mediator = mediator;
		_logger = logger;
	}

	protected void FireAndForget(Func<Task> action)
	{
		_ = Task.Run(async () =>
		{
			try
			{
				await action();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
			}
		});
	}

	public Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
		where TEvent : IEvent
	{
		FireAndForget(() => _mediator.Publish(@event, cancellationToken));
		return Task.CompletedTask;
	}
}
