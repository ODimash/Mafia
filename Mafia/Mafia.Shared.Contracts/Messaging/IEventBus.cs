namespace Mafia.Shared.Contracts.Messaging;

public interface IEventBus
{
	Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default) 
		where TEvent : IEvent;
}
