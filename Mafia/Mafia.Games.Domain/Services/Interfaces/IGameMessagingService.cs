using FluentResults;
using Mafia.Games.Domain.Models;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Games.Domain.Services.Interfaces;

public interface IGameMessagingService
{
    Result<GameChat> GetChatHeCanTell(Game game, Guid playerId);
}
