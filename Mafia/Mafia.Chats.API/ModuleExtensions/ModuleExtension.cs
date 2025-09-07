using Mafia.Chats.Application.Contracts;
using Mafia.Chats.Application.Service;
using Mafia.Chats.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Mafia.Chats.API.ModuleExtensions;
public static class ChatModuleExtensions
{
    public static IServiceCollection AddChatModule(this IServiceCollection services)
    {

        services.AddSingleton<IChatRepository, InMemoryChatRepository>();

        services.AddScoped<ChatService>();
        

        return services;
    }
}
