using Mafia.Shared.Kernel.Enums;

namespace Mafia.Games.Contracts.DTOs;

public class GameDto
{
	public required GameSettingsDto Settings { get; set; }
	public int Day { get; private set; } = 0;
	public required IReadOnlyList<PlayerDto> Players { get; set; }
	public required GamePhaseDto CurrentPhase { get; set; }
	public bool IsFinished { get; set; }
	public SideType? WinnerSide { get; set; }
}
