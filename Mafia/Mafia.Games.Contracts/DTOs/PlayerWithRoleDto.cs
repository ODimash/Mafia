using Mafia.Shared.Kernel.Enums;

namespace Mafia.Games.Contracts.DTOs;

public class PlayerWithRoleDto
{
	public Guid IdentityId { get; set; }
	public Role Role { get; set; }
	public bool IsKilled { get; set; }
	public bool IsWinner { get; set; }
}
