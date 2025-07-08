
using Mafia.Shared.Kernel;

namespace Mafia.Game.Domain.ValueObjects;

public class GameSettings : ValueObject
{
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}
