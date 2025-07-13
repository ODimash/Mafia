
using MediatR;

namespace Mafia.Game.Abstraction.Messaging
{
    public interface ICommand<out TResponse> : IRequest<TResponse>  { }
    public interface IQuery<out TResponse> : IRequest<TResponse> { }
    public interface IEvent : INotification { }

}
