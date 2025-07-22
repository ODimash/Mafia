using FluentResults;
using Mafia.Shared.Contracts.Messaging;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Games.Contracts.Queries;

public record GetPlayerCanTellChatQuery(Guid PlayerId, Guid GameId) : IQuery<Result<GameChat>>;
