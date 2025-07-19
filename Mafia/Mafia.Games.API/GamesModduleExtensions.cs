using Mafia.Games.Abstraction.Repositories;
using Mafia.Games.API.Controllers;
using Mafia.Games.Application.Handlers.GameHandlers;
using Mafia.Games.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Mafia.Games.API;

public static class GamesModduleExtensions
{
    public static IServiceCollection AddGamesModule(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddScoped<IGameCommandRepository, GameCommandRepository>();
        services.AddEndpointsApiExplorer();
        services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(StartGameHandler).Assembly));
        
        return services;
    }
}

