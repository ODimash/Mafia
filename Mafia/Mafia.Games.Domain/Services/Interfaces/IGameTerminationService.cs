using Mafia.Games.Domain.Models;

namespace Mafia.Games.Domain.Services.Interfaces;

public interface IGameTerminationService
{
	bool TryTerminateGame(Game game);
}
