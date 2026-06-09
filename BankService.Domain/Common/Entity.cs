namespace BankService.Domain.Common;

/// <summary>
/// Base class for entities: objects with a distinct identity that runs through
/// time and different states. Two entities are considered equal when their
/// identifiers are equal, regardless of their other attribute values.
/// </summary>
public abstract class Entity : IEquatable<Entity>
{
    public Guid Id { get; protected set; }

    protected Entity(Guid id) => Id = id;

    // Parameterless ctor supports reconstitution (e.g. the Restore factory).
    protected Entity()
    {
    }

    public bool Equals(Entity? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return GetType() == other.GetType() && Id.Equals(other.Id);
    }

    public override bool Equals(object? obj) => obj is Entity entity && Equals(entity);

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(Entity? left, Entity? right) => Equals(left, right);

    public static bool operator !=(Entity? left, Entity? right) => !Equals(left, right);
}
