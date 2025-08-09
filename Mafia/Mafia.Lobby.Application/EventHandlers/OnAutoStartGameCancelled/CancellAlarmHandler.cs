using Mafia.Lobby.Domain.DomainEvents;
using Mafia.Shared.Contracts.Messaging;
using Mafia.Shared.Kernel.Services;

namespace Mafia.Lobby.Application.EventHandlers.OnAutoStartGameCancelled;

public class CancellAlarmHandler : IDomainEventHandler<AutoStartGameCancelledDomainEvent>
{
	private readonly IAlarmService _alarmService;

	public CancellAlarmHandler(IAlarmService alarmService)
	{
		_alarmService = alarmService;
	}

	public Task Handle(DomainEventNotification<AutoStartGameCancelledDomainEvent> notification, CancellationToken cancellationToken)
	{
		_alarmService.CancellAlarmsByEventTipe(typeof(TimerToStartGameFinishedDomainEvent));
		return Task.CompletedTask;
	}
}
