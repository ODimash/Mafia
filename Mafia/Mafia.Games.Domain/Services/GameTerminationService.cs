using Mafia.Games.Domain.Models;
using Mafia.Games.Domain.Services.Interfaces;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Games.Domain.Services;

public class GameTerminationService : IGameTerminationService
{

	public bool TryTerminateGame(Game game)
	{
		var alivePlayers = game.Players.Where(p => !p.IsKilled).ToList();
		var mafiaCount = alivePlayers.Count(p => p.Role.GetSide() == SideType.MafiaTeam);
		if (mafiaCount <= 0)
		{
			game.FinishGame(SideType.CivilianTeam);
			return true;
		}
		
		var civilianCount = alivePlayers.Count(p => p.Role.GetSide() == SideType.CivilianTeam);
		if (mafiaCount >= civilianCount)
		{
			game.FinishGame(SideType.MafiaTeam);
			return true;
		}
		
		return false;
	}
}
