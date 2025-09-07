using Mafia.Shared.Kernel.Enums;

namespace Mafia.Shared.Contracts.Models.DTOs.Lobby;

public class RoomSettingsDto
{
	public int DayDiscussionDurationInSeconds { get; set; }
	public int NightDurationInSeconds { get; set; }
	public int VotingDurationInSeconds { get; set; }
	public int MaxPlayersCount { get; set; }
	public int MinPlayersCount { get; set; }

	public required IReadOnlyList<Role> EnabledRoles { get; set; }
}
