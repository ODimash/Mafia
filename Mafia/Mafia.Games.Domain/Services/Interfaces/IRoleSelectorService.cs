using Mafia.Games.Domain.Models;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Games.Domain.Services.Interfaces;

public interface IRoleSelectorService
{
	List<(Guid, Role)> SelectRoles(List<Guid> userIds, GameSettings settings);
}
