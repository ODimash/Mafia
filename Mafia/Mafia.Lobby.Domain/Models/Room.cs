
using Mafia.Shared.Kernel;

namespace Mafia.Lobby.Domain.Models;
public class Room : AggregateRoot<Guid>
{
    private readonly List<Player> _players = [];

    public IReadOnlyList<Player> Players => _players;
    public RoomSettings Settings { get; private set; }
    public Guid OwnerId { get; private set; }
    public string Password { get; private set; }
    public RoomCode Code { get; private set; }

}
