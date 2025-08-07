using Mafia.Shared.Kernel.Enums;

namespace Mafia.Lobby.Contracts.DTOs;

public class RoomSettingsDto
{
	public TimeSpan DayDiscussionDuration { get; set; }
	public TimeSpan NightDuration { get; set; }
	public TimeSpan VotingDuration { get; set; }
	public int MaxPlayersCount { get; set; }
	public int MinPlayersCount { get; set; }

	public required List<Role> EnabledRoles { get; set; } 
}
