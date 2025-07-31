using Mafia.Games.Contracts.DTOs;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Games.Application.Handlers.PlayerHandlers.GetPlayers;

public record GetPlayersQuery(Guid GameId) : IQuery<List<PlayerDto>>;
