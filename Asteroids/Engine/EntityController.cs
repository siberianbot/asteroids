using Asteroids.Commands;
using Asteroids.Entities;

namespace Asteroids.Engine;

public class EntityController
{
    private readonly CommandQueue _commandQueue;
    private readonly DependencyContainer _dependencyContainer;
    private readonly List<Entity> _entities;

    public EntityController(CommandQueue commandQueue, DependencyContainer dependencyContainer)
    {
        _commandQueue = commandQueue;
        _dependencyContainer = dependencyContainer;
        _entities = new List<Entity>();
    }

    public void AddEntity(Entity entity)
    {
        _entities.Add(entity);
    }

    public void ForEachEntity(Action<Entity> action)
    {
        foreach (Entity entity in _entities)
        {
            action(entity);
        }
    }

    public IEnumerable<Entity> Entities
    {
        get => _entities;
    }

    public void Clear()
    {
        _entities.Clear();
    }

    public void Destroy(Entity entity)
    {
        DestroyContext destroyContext = new DestroyContext
        {
            DependencyContainer = _dependencyContainer
        };

        entity.Destroy(destroyContext);

        _commandQueue.Push(() => _entities.Remove(entity));
    }

    public IEnumerable<TOwnedEntity> GetOwnedEntities<TOwnedEntity>(Entity owner)
        where TOwnedEntity : Entity, IOwnedEntity
    {
        return _entities
            .OfType<TOwnedEntity>()
            .Where(entity => entity.Owner == owner);
    }
}