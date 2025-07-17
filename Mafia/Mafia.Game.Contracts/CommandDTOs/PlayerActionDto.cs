

using Mafia.Game.Domain.Enums;

namespace Mafia.Game.Contracts.DTOs;

public class PlayerActionDto
{
    public Guid ActorId { get; }
    public Guid TargetId { get; }
    public ActionType ActionType { get; }
}
