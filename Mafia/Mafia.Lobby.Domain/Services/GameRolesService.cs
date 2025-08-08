using FluentResults;
using Mafia.Shared.Kernel.Constants;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Lobby.Domain.Services;

public interface IGameRolesService
{
	Result<List<Role>> GenerateRolesOfEnabledRoles(IReadOnlyList<Role> enabledRoles, int playersCount);
}

public class GameRolesService : IGameRolesService
{
	public Result<List<Role>> GenerateRolesOfEnabledRoles(IReadOnlyList<Role> enabledRoles, int playersCount)
	{
		if (enabledRoles.Count == 0)
			return Result.Fail("Enabled roles cannot be null or empty");
		if (playersCount < GameSettingConstants.MinPlayersCount) 
			return Result.Fail("Too few players");

		var result = new List<Role>();

		int mafiaCount = Math.Max(1, playersCount / 4);
		if (!enabledRoles.Contains(Role.Mafia))
			mafiaCount = 0;

		for (int i = 0; i < mafiaCount; i++)
			result.Add(Role.Mafia);

		// Добавляем особые роли (по одной штуке, если они включены)
		foreach (var specialRole in new[] { Role.Sheriff, Role.Doctor })
		{
			if (enabledRoles.Contains(specialRole) && result.Count < playersCount)
				result.Add(specialRole);
		}

		// Заполняем оставшееся место мирными
		while (result.Count < playersCount)
		{
			if (enabledRoles.Contains(Role.Civil))
				result.Add(Role.Civil);
			else
				break;
		}
		
		return result;
	}

}
