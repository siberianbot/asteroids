using Asteroids.Engine;
using Asteroids.Entities;

namespace Asteroids.Controllers;

public class PlayerController : IController
{
    private readonly CommandQueue _commandQueue;
    private readonly List<Player> _players = new List<Player>();

    public PlayerController(CommandQueue commandQueue, EventQueue eventQueue)
    {
        _commandQueue = commandQueue;

        eventQueue.OnEvent(EventType.EntityDestroy, @event =>
        {
            if (@event.Entity is not Player player)
            {
                return;
            }

            RemovePlayer(player);
        });
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