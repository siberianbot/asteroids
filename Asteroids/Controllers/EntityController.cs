using Asteroids.Engine;
using Asteroids.Entities;

namespace Asteroids.Controllers;

public class EntityController : IController, IEntityCollection
{
    private readonly EventQueue _eventQueue;
    private readonly List<Entity> _entities = new List<Entity>();
    private readonly object _entitiesLock = new object();
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
            lock (_entitiesLock)
            {
                _entities.Add(@event.Entity!);
            }

            @event.Entity!.Create();
        });

        _entityDestroySubscription = _eventQueue.Subscribe(EventType.EntityDestroy, @event =>
        {
            @event.Entity!.Destroy();

            lock (_entitiesLock)
            {
                _entities.Remove(@event.Entity!);
            }
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
        get
        {
            lock (_entitiesLock)
            {
                return _entities.ToArray();
            }
        }
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

    // TODO: this method is used only for retrieving single entity, is it really required to retrieve all entities?
    public IEnumerable<TOwnedEntity> GetOwnedEntities<TOwnedEntity>(Entity owner)
        where TOwnedEntity : Entity, IOwnedEntity
    {
        lock (_entitiesLock)
        {
            return _entities
                .OfType<TOwnedEntity>()
                .Where(entity => entity.Owner == owner)
                .ToArray();
        }
    }

    public void Reset()
    {
        lock (_entitiesLock)
        {
            _entities.Clear();
        }
    }
}