using Asteroids.Engine;
using Asteroids.Entities;

namespace Asteroids.Controllers;

public class PlayerController : IController
{
    private readonly EventQueue _eventQueue;
    private readonly List<Player> _players = new List<Player>();
    private long _entitySpawnSubscription;
    private long _entityDestroySubscription;
    private long _sceneChangeSubscription;

    public PlayerController(EventQueue eventQueue)
    {
        _eventQueue = eventQueue;
    }

    public void Initialize()
    {
        _entitySpawnSubscription = _eventQueue.Subscribe(EventType.EntitySpawn, @event =>
        {
            if (@event.Entity is not Player player)
            {
                return;
            }

            _players.Add(player);
        });

        _entityDestroySubscription = _eventQueue.Subscribe(EventType.EntityDestroy, @event =>
        {
            if (@event.Entity is not Player player)
            {
                return;
            }

            _players.Remove(player);
        });

        _sceneChangeSubscription = _eventQueue.Subscribe(EventType.SceneChange, _ => Reset());
    }

    public void Terminate()
    {
        _eventQueue.Unsubscribe(EventType.EntitySpawn, _entitySpawnSubscription);
        _eventQueue.Unsubscribe(EventType.EntityDestroy, _entityDestroySubscription);
        _eventQueue.Unsubscribe(EventType.SceneChange, _sceneChangeSubscription);
    }

    public IReadOnlyCollection<Player> Players
    {
        get => _players;
    }

    public void Reset()
    {
        _players.Clear();
    }
}