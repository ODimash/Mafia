using Mafia.Games.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Mafia.Games.Infrastructure.BackgroundServices;

public class AlarmBackgroundService : BackgroundService
{
	private readonly IServiceProvider _serviceProvider;
	private readonly ILogger<AlarmBackgroundService> _logger;

	public AlarmBackgroundService(IServiceProvider serviceProvider, ILogger<AlarmBackgroundService> logger)
	{
		_serviceProvider = serviceProvider;
		_logger = logger;
	}

	async protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			var dueEvents = InMemoryAlarmService.GetDueEvents();

			foreach (var domainEvent in dueEvents)
			{
				try
				{
					using var scope = _serviceProvider.CreateScope();
					var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
					await mediator.Publish(domainEvent.Event, stoppingToken);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Ошибка при публикации запланированного события");
				}
			}

			await Task.Delay(1000, stoppingToken);
		}
	}
}
