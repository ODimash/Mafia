﻿
namespace Mafia.Shared.Kernel;
public abstract class Entity<TId>
{
    public TId Id { get; protected set; } = default!;
    public DateTime CreatedAt { get; protected set; }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TId> other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return Id!.Equals(other.Id);
    }

    public override int GetHashCode() => Id!.GetHashCode();
}