using Mafia.Shared.Kernel.Enums;

namespace Mafia.Shared.Contracts.DTOs.Games;

public class PlayerActionsToDoDto
{
	public bool IsPerformed { get; set; }
	public PlayerActionDto? PerformedAction { get; set; }
	public List<ActionType>? ActionsToDo { get; set; }
}
