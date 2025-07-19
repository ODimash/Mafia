
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Game.Domain.ValueObjects
{
    public class AppliedAction(
        Guid targetId, 
        ActionType actionType, 
        bool isPrivate = false,
        string? additionalInformation = null)
    {
        public Guid TargetId { get; } = targetId;
        public ActionType ActionType { get; } = actionType;
        public bool IsPrivate { get; } = isPrivate;
        public string? AdditionalInformation { get; } = additionalInformation;
    }
}
