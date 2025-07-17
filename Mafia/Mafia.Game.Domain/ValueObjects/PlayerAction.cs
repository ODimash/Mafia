using FluentResults;
using Mafia.Game.Domain.Entities;
using Mafia.Game.Domain.Enums;
using Mafia.Shared.Kernel;

namespace Mafia.Game.Domain.ValueObjects;

public class PlayerAction : ValueObject
{
    public DateTime CreatedAt { get; }
    public Guid ActorId { get; }
    public Guid TargetId { get; }
    public ActionType ActionType { get; }

    private PlayerAction(Guid actorId, Guid targetId, ActionType actionType, DateTime createdAt)
    {
        CreatedAt = createdAt;
        ActorId = actorId;
        TargetId = targetId;
        ActionType = actionType;
    }

    public static Result<PlayerAction> Create(Player actor, Player? target, ActionType actionType)
    {
        if (actor.IsKilled)
            return Result.Fail("Died player can not make action");

        if (target != null && target.IsKilled)
            return Result.Fail("You can not make something with died player");

        if (!actor.Role.GetAvailableActionByRole().Contains(actionType))
            return Result.Fail($"{actor.Role} can not ${actionType}");

        if (actor.LastAction != null)
            return Result.Fail("Actions have already been completed");

        return Result.Ok(new PlayerAction(actor.Id, target?.Id ?? Guid.Empty, actionType, DateTime.UtcNow));
    }


    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return TargetId;
        yield return ActionType;
    }
}