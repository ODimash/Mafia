

using FluentResults;
using Mafia.Shared.Kernel;
using MediatR;

namespace Mafia.Shared.Contracts.Messaging;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TResponse : IResultBase
{ }

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent>
    where TEvent : IEvent;

public interface IDomainEventHandler<TDomainEvent> : INotificationHandler<DomainEventNotification<TDomainEvent>>
    where TDomainEvent : DomainEvent;