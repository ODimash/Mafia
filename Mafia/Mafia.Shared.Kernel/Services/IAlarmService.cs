namespace Mafia.Shared.Kernel.Services;

public interface IAlarmService
{
	void SetAlarm(DateTime time, IDomainEvent domainEvent);
	void SetTimer(TimeSpan interval, IDomainEvent domainEvent);
	void CancellAlarmsByEventTipe(Type type);
}
