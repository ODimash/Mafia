using Mafia.User.Application.Contracts;
using Mafia.User.Application.Services;
using Mafia.User.Infrastructure.Repositories;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;


namespace Mafia.User.API
{
    public static class ModuleExtensions
    {
        // Регистрируем зависимости
        public static IServiceCollection AddUsersModule(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<UserService>();
           
            return services;
        }

    }
}
