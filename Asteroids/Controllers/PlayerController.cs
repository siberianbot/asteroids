using Asteroids.Engine;
using Asteroids.Entities;

namespace Asteroids.Controllers;

public class PlayerController : IController
{
    private readonly CommandQueue _commandQueue;
    private readonly EventQueue _eventQueue;
    private readonly List<Player> _players = new List<Player>();
    private long _entityDestroySubscription;
    private long _sceneChangeSubscription;

    public PlayerController(CommandQueue commandQueue, EventQueue eventQueue)
    {
        _commandQueue = commandQueue;
        _eventQueue = eventQueue;
    }

    public void Initialize()
    {
        _entityDestroySubscription = _eventQueue.Subscribe(EventType.EntityDestroy, @event =>
        {
            if (@event.Entity is not Player player)
            {
                return;
            }

            RemovePlayer(player);
        });

        _sceneChangeSubscription = _eventQueue.Subscribe(EventType.SceneChange, _ => Reset());
    }

    public void Terminate()
    {
        _eventQueue.Unsubscribe(EventType.EntityDestroy, _entityDestroySubscription);
        _eventQueue.Unsubscribe(EventType.SceneChange, _sceneChangeSubscription);
    }

    public IReadOnlyCollection<Player> Players
    {
        get => _players;
    }

    public void AddPlayer(Player player)
    {
        _commandQueue.Push(() => _players.Add(player));
    }

    public void RemovePlayer(Player player)
    {
        _commandQueue.Push(() => _players.Remove(player));
    }

    public void Reset()
    {
        _players.Clear();
    }
}