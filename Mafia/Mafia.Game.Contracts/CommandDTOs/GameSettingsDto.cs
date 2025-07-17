

using Mafia.Game.Domain.Enums;

namespace Mafia.Game.Contracts.DTOs;

public class GameSettingsDto
{
    public TimeSpan DayDiscussionDuration { get; set; }
    public TimeSpan NightDuration { get; set; }
    public TimeSpan VoteDuration { get; set; }
    public List<Role> Roles { get; set; }
}
