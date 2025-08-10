using MediatR;

namespace Mafia.Lobby.Application.EventHandlers.OnPlayerConnectionLost;

public class UserConnectionLostEvent : INotification
{
	public UserConnectionLostEvent(Guid userId)
	{
		UserId = userId;
	}
	public Guid UserId { get; }
	public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
