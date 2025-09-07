using Mafia.Shared.Kernel.Enums;

namespace Mafia.Shared.Contracts.DTOs.Games;

public class GameSettingsDto
{
    public required TimeSpan DayDiscussionDuration { get; set; }
    public required TimeSpan NightDuration { get; set; }
    public required TimeSpan VoteDuration { get; set; }
    public required List<Role> Roles { get; set; }
}