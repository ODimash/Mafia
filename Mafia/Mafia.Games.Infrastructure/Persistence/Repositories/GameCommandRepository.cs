using Mafia.Games.Abstraction.Repositories;
using Mafia.Games.Domain.Entities;
using System.Collections.Concurrent;

namespace Mafia.Games.Infrastructure.Persistence.Repositories;

public class GameCommandRepository : IGameCommandRepository
{
    private readonly ConcurrentDictionary<Guid, Game> _games = [];

    public Task AddGame(Game game, CancellationToken cancellationToken)
    {
        _games.TryAdd(game.Id, game);
        return Task.CompletedTask;
    }
    public Task<Game?> GetGameById(Guid requestGameId, CancellationToken cancellationToken)
    {
        return Task.FromResult(_games.TryGetValue(requestGameId, out var game) ? game : null);
    }
    public Task Update(Game game, CancellationToken cancellationToken)
    {
        _games[game.Id] = game;
        return Task.CompletedTask;
    }
}
