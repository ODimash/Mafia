
using FluentResults;
using MediatR;

namespace Mafia.Shared.Contracts.Messaging;

public interface ICommand<out TResponse> : IRequest<TResponse> where TResponse : IResultBase { }
public interface IQuery<out TResponse> : IRequest<TResponse> { }
public interface IEvent : INotification { }
