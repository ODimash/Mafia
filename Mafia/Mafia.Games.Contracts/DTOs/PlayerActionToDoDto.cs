using Mafia.Shared.Kernel.Enums;
using System.Diagnostics.CodeAnalysis;

namespace Mafia.Games.Contracts.DTOs;

public class PlayerActionsToDoDto
{
	public bool IsPerformed { get; set; }
	public PlayerActionDto? PerformedAction { get; set; }
	public List<ActionType>? ActionsToDo { get; set; }
}
