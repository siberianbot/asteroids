using Asteroids.Engine;
using Asteroids.Entities;

namespace Asteroids.Controllers;

public class EntityController : IController, IEntityCollection
{
    private readonly EventQueue _eventQueue;
    private readonly List<Entity> _entities = new List<Entity>();
    private long _sceneChangeSubscription;
    private long _entitySpawnSubscription;
    private long _entityDestroySubscription;

    public EntityController(EventQueue eventQueue)
    {
        _eventQueue = eventQueue;
    }

    public void Initialize()
    {
        _sceneChangeSubscription = _eventQueue.Subscribe(EventType.SceneChange, _ => Reset());

        _entitySpawnSubscription = _eventQueue.Subscribe(EventType.EntitySpawn, @event =>
        {
            _entities.Add(@event.Entity!);

            @event.Entity!.Create();
        });

        _entityDestroySubscription = _eventQueue.Subscribe(EventType.EntityDestroy, @event =>
        {
            @event.Entity!.Destroy();

            _entities.Remove(@event.Entity!);
        });
    }

    public void Terminate()
    {
        _eventQueue.Unsubscribe(EventType.SceneChange, _sceneChangeSubscription);
        _eventQueue.Unsubscribe(EventType.EntitySpawn, _entitySpawnSubscription);
        _eventQueue.Unsubscribe(EventType.EntityDestroy, _entityDestroySubscription);
    }

    public IReadOnlyCollection<Entity> Entities
    {
        get => _entities;
    }

    public void AddEntity(Entity entity)
    {
        _eventQueue.Push(new Event
        {
            EventType = EventType.EntitySpawn,
            Entity = entity
        });
    }

    public void DestroyEntity(Entity entity)
    {
        _eventQueue.Push(new Event
        {
            EventType = EventType.EntityDestroy,
            Entity = entity
        });
    }

    public IEnumerable<TOwnedEntity> GetOwnedEntities<TOwnedEntity>(Entity owner)
        where TOwnedEntity : Entity, IOwnedEntity
    {
        return _entities
            .OfType<TOwnedEntity>()
            .Where(entity => entity.Owner == owner);
    }

    public void Reset()
    {
        _entities.Clear();
    }
}