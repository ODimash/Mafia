using Mafia.Lobby.Abstraction.Notifiers;
using Mafia.Lobby.Abstraction.Repositories;
using Mafia.Lobby.API.Hubs.Notifiers;
using Mafia.Lobby.Application.Handlers.RoomHandlers.StartGame;
using Mafia.Lobby.Domain.Services;
using Mafia.Lobby.DTO.Mappers;
using Mafia.Lobby.Infrasctructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mafia.Lobby.API;

public static class LobbyModuleExtensions
{
	public static IServiceCollection AddLobbyModule(this IServiceCollection services)
	{
		services
			.AddSingleton<ILobbyNotifier, LobbyNotifier>()
			.AddSingleton<IRoomNotifier, RoomNotifier>()
			.AddSingleton<InMemoryRoomRepository>() 
			.AddSingleton<IRoomRepository>(sp => sp.GetService<InMemoryRoomRepository>()!)
			.AddSingleton<IRoomQueryRepository, InMemoryRoomQueryRepository>()
			.AddSingleton<IGameRolesService, GameRolesService>();

		services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(StartGameHandler).Assembly));
		services.AddAutoMapper(c => c.AddProfile<DtoProfile>());
		
		return services;
	}
}
