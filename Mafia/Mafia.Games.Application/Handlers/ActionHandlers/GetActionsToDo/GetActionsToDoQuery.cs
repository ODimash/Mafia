using FluentResults;
using Mafia.Shared.Contracts.DTOs;
using Mafia.Shared.Contracts.DTOs.Games;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Games.Application.Handlers.ActionHandlers.GetActionsToDo;

public record GetActionsToDoQuery(Guid GameId, Guid PlayerId) : IQuery<Result<PlayerActionsToDoDto>>;