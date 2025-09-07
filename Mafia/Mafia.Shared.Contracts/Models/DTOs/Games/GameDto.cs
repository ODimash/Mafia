using Mafia.Shared.Contracts.DTOs.Games;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Shared.Contracts.Models.DTOs.Games;

public class GameDto
{
	public required GameSettingsDto Settings { get; set; }
	public int Day { get; private set; } = 0;
	public required IReadOnlyList<PlayerDto> Players { get; set; }
	public required GamePhaseDto CurrentPhase { get; set; }
	public bool IsFinished { get; set; }
	public SideType? WinnerSide { get; set; }
}
