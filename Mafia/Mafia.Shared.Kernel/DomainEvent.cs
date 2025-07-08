namespace Mafia.Shared.Kernel;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}

public abstract class DomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; private set; } = DateTime.UtcNow;
}
