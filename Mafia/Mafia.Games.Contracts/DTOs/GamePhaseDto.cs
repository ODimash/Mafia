using Mafia.Shared.Kernel.Enums;

namespace Mafia.Games.Contracts.DTOs;

public class GamePhaseDto
{
	public PhaseType Type { get; set; }
	public required IReadOnlyList<Guid> PlayersForAction { get; set; }
	public required IReadOnlyList<PlayerActionDto> PerfectActions { get; set; }
	public DateTime EndTime { get; set; }
}
