using Mafia.Games.Domain.Services.Interfaces;

namespace Mafia.Games.Domain.Services;

public class ClockService : IClockService
{
    public DateTime CurrentDateTime =>  DateTime.UtcNow;
}
