using Asteroids.Engine;
using Asteroids.Entities;

namespace Asteroids.Controllers;

public class EntityController : IController
{
    private readonly CommandQueue _commandQueue;
    private readonly EventQueue _eventQueue;
    private readonly List<Entity> _entities = new List<Entity>();
    private long _sceneChangeSubscription;

    public EntityController(CommandQueue commandQueue, EventQueue eventQueue)
    {
        _commandQueue = commandQueue;
        _eventQueue = eventQueue;
    }

    public void Initialize()
    {
        _sceneChangeSubscription = _eventQueue.Subscribe(EventType.SceneChange, _ => Reset());
    }

    public void Terminate()
    {
        _eventQueue.Unsubscribe(EventType.SceneChange, _sceneChangeSubscription);
    }

    public IReadOnlyCollection<Entity> Entities
    {
        get => _entities;
    }

    public void AddEntity(Entity entity)
    {
        _commandQueue.Push(() =>
        {
            _entities.Add(entity);

            entity.Create();
        });
    }

    public void DestroyEntity(Entity entity)
    {
        _commandQueue.Push(() =>
        {
            entity.Destroy();

            _entities.Remove(entity);
        });

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