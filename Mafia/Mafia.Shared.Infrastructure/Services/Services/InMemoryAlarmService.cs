using Mafia.Shared.Kernel;
using Mafia.Shared.Kernel.Services;
using Microsoft.Extensions.Logging;

namespace Mafia.Shared.Infrastructure.Services.Services;

public class InMemoryAlarmService : IAlarmService
{
	private readonly ILogger<InMemoryAlarmService> _logger;
	private static readonly List<ScheduledEvent> _events = [];
	private static readonly object _lock = new();

	public InMemoryAlarmService(ILogger<InMemoryAlarmService> logger)
	{
		_logger = logger;
	}

	public void SetAlarm(DateTime time, IDomainEvent domainEvent)
	{
		lock (_lock)
		{
			_events.Add(new ScheduledEvent
			{
				ExecuteAt = time,
				Event = domainEvent
			});
		}
	}

	public void SetTimer(TimeSpan interval, IDomainEvent domainEvent)
	{
		SetAlarm(DateTime.UtcNow + interval, domainEvent);
	}
	public void CancellAlarmsByEventTipe(Type type)
	{
		lock (_lock)
		{
			_events.RemoveAll(x => x.Event.GetType() == type);
		}
	}

	// Вызывается из фонового сервиса
	public static List<ScheduledEvent> GetDueEvents()
	{
		lock (_lock)
		{
			var now = DateTime.UtcNow;
			var due = _events.Where(e => e.ExecuteAt <= now).ToList();
			_events.RemoveAll(e => e.ExecuteAt <= now);
			return due;
		}
	}

	public class ScheduledEvent
	{
		public DateTime ExecuteAt { get; set; }
		public required IDomainEvent Event { get; set; }

		internal ScheduledEvent()
		{
		}
	}
}
