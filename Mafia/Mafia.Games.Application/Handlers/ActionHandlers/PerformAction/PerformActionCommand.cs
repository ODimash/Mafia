using FluentResults;
using Mafia.Shared.Contracts.Messaging;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Games.Application.Handlers.ActionHandlers.PerformAction;

public record PerformActionCommand(Guid GameId, Guid ActorId, Guid TargetId, ActionType ActionType) : ICommand<Result> { };

