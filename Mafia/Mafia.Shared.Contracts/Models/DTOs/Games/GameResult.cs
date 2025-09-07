using Mafia.Shared.Kernel.Enums;

namespace Mafia.Shared.Contracts.DTOs.Games;

public class GameResult
{
	public required Guid GameId { get; set; }
	public required SideType WinnerSide { get; set; }
	public required List<PlayerWithRoleDto> Players { get; set; }
	public required int PassedDays { get; set; }
}
