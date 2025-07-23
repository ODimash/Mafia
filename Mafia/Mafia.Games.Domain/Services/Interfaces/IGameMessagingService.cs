using FluentResults;
using Mafia.Games.Domain.Entities;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Games.Domain.Services.Interfaces;

public interface IGameMessagingService
{
    Result<GameChat> GetChatHeCanTell(Game game, Guid playerId);
}
