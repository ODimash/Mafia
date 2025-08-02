using Mafia.Games.Abstraction;
using Mafia.Shared.Kernel;
using Microsoft.Extensions.Logging;

namespace Mafia.Games.Infrastructure.Services;

public class InMemoryAlarmService : IAlarmService
{
	private readonly ILogger<InMemoryAlarmService> _logger;
	private readonly IServiceProvider _serviceProvider;
	private static readonly List<ScheduledEvent> _events = [];
	private static readonly object _lock = new();

	public InMemoryAlarmService(ILogger<InMemoryAlarmService> logger, IServiceProvider serviceProvider)
	{
		_logger = logger;
		_serviceProvider = serviceProvider;
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
