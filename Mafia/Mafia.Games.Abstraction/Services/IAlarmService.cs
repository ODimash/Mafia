using Mafia.Shared.Kernel;

namespace Mafia.Games.Abstraction.Services;

public interface IAlarmService
{
	void SetAlarm(DateTime time, IDomainEvent domainEvent);
	void SetTimer(TimeSpan interval, IDomainEvent domainEvent);
}
