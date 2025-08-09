using Mafia.Lobby.Domain.DomainEvents;
using Mafia.Shared.Contracts.Messaging;
using Mafia.Shared.Kernel.Services;

namespace Mafia.Lobby.Application.EventHandlers.OnGameWillStart;

public class SetAlarmHandler : IDomainEventHandler<GameWillStartDomainEvent>
{
	private readonly IAlarmService  _alarmService;
	
	public SetAlarmHandler(IAlarmService alarmService)
	{
		_alarmService = alarmService;
	}

	public Task Handle(DomainEventNotification<GameWillStartDomainEvent> notification, CancellationToken cancellationToken)
	{	
		_alarmService.SetAlarm(
			notification.DomainEvent.StartTime, 
			new TimerToStartGameFinishedDomainEvent(notification.DomainEvent.RoomId));
		return Task.CompletedTask;
	}
}
