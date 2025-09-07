using Mafia.Shared.Contracts.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Windows.Input;

namespace Mafia.Shared.Infrastructure.Messaging;

public class MediatorMessageBus : MediatorEventBus, IMessageBus
{
	private readonly IMediator _mediator;

	public MediatorMessageBus(IMediator mediator, ILogger<MediatorMessageBus> logger)
		: base(mediator, logger)
	{
		_mediator = mediator;
	}

	public Task Send<TCommand>(TCommand command, CancellationToken cancellationToken = default)
		where TCommand : ICommand
	{
		FireAndForget(async () => await _mediator.Send(command, cancellationToken));
		return Task.CompletedTask;
	}
}
