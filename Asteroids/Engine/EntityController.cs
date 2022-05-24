using Asteroids.Commands;
using Asteroids.Entities;

namespace Asteroids.Engine;

public class EntityController
{
    private readonly CommandQueue _commandQueue;
    private readonly List<Entity> _entities;

    public EntityController(CommandQueue commandQueue)
    {
        _commandQueue = commandQueue;
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

    public void Clear()
    {
        _entities.Clear();
    }

    public void Destroy(Entity entity)
    {
        _commandQueue.Push(() => _entities.Remove(entity));
    }
}