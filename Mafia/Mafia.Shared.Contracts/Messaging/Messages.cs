
using FluentResults;
using MediatR;
using Mafia.Shared.Kernel;

namespace Mafia.Shared.Contracts.Messaging;

public interface ICommand<out TResponse> : IRequest<TResponse> where TResponse : IResultBase { }
public interface IQuery<out TResponse> : IRequest<TResponse> { }
public interface IEvent : INotification { }

public class DomainEventNotification<TDomainEvent> : INotification
	where TDomainEvent : DomainEvent
{
	public TDomainEvent DomainEvent { get; }

	public DomainEventNotification(TDomainEvent domainEvent)
	{
		DomainEvent = domainEvent;
	}
}