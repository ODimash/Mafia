using Mafia.User.API.Authentication;
using Mafia.User.Application.Contracts;
using Mafia.User.Application.Services;
using Mafia.User.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mafia.User.API;

public static class ModuleExtensions
{
    // Регистрируем зависимости
    public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration configuration)
    {
		services.AddTransient<IConfigureOptions<SwaggerGenOptions>, AuthSwaggerConfig>();

		services.AddSingleton<IUserRepository, InMemoryUserRepository>();
        services.AddScoped<UserService>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();

        services.AddUsersAuthentication(configuration);
        
        return services;
    }

}