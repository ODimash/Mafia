using FluentResults;
using Mafia.Games.Contracts.DTOs;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Games.Application.Handlers.ActionHandlers.GetActionsToDo;

public record GetActionsToDoQuery(Guid GameId, Guid PlayerId) : IQuery<Result<PlayerActionsToDoDto>>;