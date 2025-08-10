using System.Windows.Input;

namespace Mafia.Shared.Contracts.Messaging;

public interface IMessageBus
{
	Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
		where TEvent : IEvent;

	Task Send<TCommand>(TCommand command, CancellationToken cancellationToken = default)
		where TCommand : ICommand;
}
