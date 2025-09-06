using Mafia.Games.Application.Handlers.ActionHandlers.PerformAction;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Games.API.RequestModels;

public record PerformActionRequest(Guid TargetId, ActionType ActionType)
{
	public PerformActionCommand ToCommand(Guid actorId, Guid gameId) => new(gameId, actorId, TargetId, ActionType );
}
