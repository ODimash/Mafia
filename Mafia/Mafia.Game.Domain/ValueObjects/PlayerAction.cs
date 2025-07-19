using FluentResults;
using Mafia.Game.Domain.Entities;
using Mafia.Shared.Kernel;
using Mafia.Shared.Kernel.Enums;

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

    public static Result<PlayerAction> Create(Guid actorId, Guid targetId, ActionType actionType)
    {
        if (actorId == Guid.Empty)
            return  Result.Fail("Actor ID cannot be empty");
        
        return new PlayerAction(actorId, targetId, actionType, DateTime.UtcNow);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ActorId;
        yield return TargetId;
        yield return ActionType;
        yield return CreatedAt;
    }
}