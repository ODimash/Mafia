
using Mafia.Game.Domain.Entities;
using Mafia.Game.Domain.Enums;
using Mafia.Shared.Kernel;

namespace Mafia.Game.Domain.ValueObjects;

public class GamePhase : ValueObject
{

    public PhaseType Type { get; }
    public List<Vote> Votes { get; }
    
    public DateTime EndTime { get; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}
