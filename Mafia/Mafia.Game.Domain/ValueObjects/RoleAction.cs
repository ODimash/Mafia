using FluentResults;
using Mafia.Game.Domain.Entities;
using Mafia.Game.Domain.Enums;
using Mafia.Shared.Kernel;

namespace Mafia.Game.Domain.ValueObjects;

public class RoleAction : ValueObject
{
    public DateTime CreatedAt { get; }
    public Guid ActorId { get; }
    public Guid TargetId { get; }
    public ActionType ActionType { get; }

    private RoleAction(Guid actorId, Guid targetId, ActionType actionType, DateTime createdAt)
    {
        CreatedAt = createdAt;
        ActorId = actorId;
        TargetId = targetId;
        ActionType = actionType;
    }

    public static Result<RoleAction> Create(Player actor, Player target, ActionType actionType)
    {
        if (actor.IsKilled)
            return Result.Fail("Died player can not make action");

        if (target.IsKilled)
            return Result.Fail("You can not make something with died player");

        if (!actor.Role.ActionTypes.Contains(actionType))
            return Result.Fail($"{actor.Role} can not ${actionType}");

        if (actor.LastAction != null)
            return Result.Fail("Actions have already been completed");

        return Result.Ok(new RoleAction(actor.Id, target.Id, actionType, DateTime.UtcNow));
    }


    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CreatedAt;
        yield return ActorId;
        yield return TargetId;
        yield return ActionType;
    }
}