using Mafia.Games.Domain.Models;
using Mafia.Games.Domain.Services.Interfaces;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Games.Domain.Services;

public class RoleSelectorService : IRoleSelectorService
{
	private readonly Random _random = new Random();
	
	public List<(Guid, Role)> SelectRoles(List<Guid> userIds,  GameSettings settings)
	{
		var result = new List<(Guid, Role)>(userIds.Count);
		var mixedRoles = MixRoles(settings.Roles);
		return mixedRoles.Zip(userIds, (role, uId) => (uId, role)).ToList();
	}

	private List<Role> MixRoles(IReadOnlyList<Role> roles)
	{
		var mixedRoles = roles.ToList();
		for (int i = 0; i < roles.Count; i++)
		{
			var randomIndex = _random.Next(mixedRoles.Count);
			(mixedRoles[i], mixedRoles[randomIndex]) = (mixedRoles[randomIndex], mixedRoles[i]);
		}
		return mixedRoles;
	}
}
