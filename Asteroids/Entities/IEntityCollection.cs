namespace Asteroids.Entities;

public interface IEntityCollection
{
    IReadOnlyCollection<Entity> Entities { get; }

    IEnumerable<TOwnedEntity> GetOwnedEntities<TOwnedEntity>(Entity owner)
        where TOwnedEntity : Entity, IOwnedEntity;
}