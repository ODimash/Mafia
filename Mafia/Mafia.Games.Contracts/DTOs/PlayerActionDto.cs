using Mafia.Shared.Kernel.Enums;

namespace Mafia.Games.Contracts.DTOs;

public class PlayerActionDto
{
    public Guid ActorId { get; set; }
    public Guid TargetId { get; set; }
    public ActionType ActionType { get; set; }
}