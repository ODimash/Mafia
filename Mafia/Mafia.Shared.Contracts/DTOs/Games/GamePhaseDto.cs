using Mafia.Shared.Kernel.Enums;

namespace Mafia.Shared.Contracts.DTOs.Games;

public class GamePhaseDto
{
	public PhaseType Type { get; set; }
	public required IReadOnlyList<Guid> PlayersForAction { get; set; }
	public required IReadOnlyList<PlayerActionDto> PerfectActions { get; set; }
	public DateTime EndTime { get; set; }
}
