using Asteroids.Engine;
using Asteroids.Entities;

namespace Asteroids.Controllers;

public class EntityController : IController
{
    private readonly CommandQueue _commandQueue;
    private readonly PlayerController _playerController;
    private readonly List<Entity> _entities = new List<Entity>();

    public EntityController(CommandQueue commandQueue, PlayerController playerController)
    {
        _commandQueue = commandQueue;
        _playerController = playerController;
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
        DestroyContext destroyContext = new DestroyContext
        {
            PlayerController = _playerController
        };

        _commandQueue.Push(() =>
        {
            entity.Destroy(destroyContext);

            _entities.Remove(entity);
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