using Mafia.Games.Abstraction.Notifiers;
using Mafia.Games.Abstraction.Repositories;
using Mafia.Games.API.Hubs.Notifiers;
using Mafia.Games.API.Tokens.GameToken;
using Mafia.Games.Application.Handlers.GameHandlers;
using Mafia.Games.Application.Mappers;
using Mafia.Games.Domain.Services;
using Mafia.Games.Domain.Services.Interfaces;
using Mafia.Games.Infrastructure.Persistence.Repositories;
using Mafia.Shared.Contracts.Messaging;
using Mafia.Shared.Kernel.Services;
using Mefia.Shared.Infrastructure.BackgroundServices;
using Mefia.Shared.Infrastructure.Messaging;
using Mefia.Shared.Infrastructure.Services.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mafia.Games.API;

public static class GamesModuleExtensions
{
	public static IServiceCollection AddGamesModule(this IServiceCollection services, IConfiguration configuration)
	{
		// Configs
		services.Configure<GameTokenOptions>(configuration.GetSection("GameToken"));

		// External Services
		services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(StartGameHandler).Assembly));
		services.AddAutoMapper(c => c.AddProfile<DtoProfile>());
		services.AddSignalR();

		// Background Services
		services.AddHostedService<AlarmBackgroundService>();

		// Репозитории
		services.AddSingleton<IGameCommandRepository, InMemoryGameCommandRepository>();
		services.AddSingleton<IGameQueryRepository, InMemoryGameQueryReposiitory>();

		// Integration Services
		services.AddSingleton<IGameTokenManager, GameTokenManager>();
		services.AddSingleton<IGameNotifier, GameNotifier>();
		services.AddSingleton<IAlarmService, InMemoryAlarmService>();
		services.AddScoped<IEventBus, MediatorEventBus>();
		services.AddScoped<IMessageBus, MediatorMessageBus>();
		
		// Domain Services
		services.AddSingleton<IClockService, ClockService>();
		services.AddSingleton<IGameActionService, GameActionService>();
		services.AddSingleton<IGameMessagingService, GameMessagingService>();
		services.AddSingleton<IGamePhaseService, GamePhaseService>();
		services.AddSingleton<IGameTerminationService, GameTerminationService>();
		services.AddSingleton<IRoleSelectorService, RoleSelectorService>();

		services.AddHttpContextAccessor();
		services.AddScoped<IGameContextAccessor, GameContextAccessor>();
		
		return services;
	}
}
