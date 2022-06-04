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

        eventQueue.Event += OnEvent;
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

    private void OnEvent(Event @event)
    {
        if (@event.EventType != EventType.EntityDestroy || @event.Entity is not Player player)
        {
            return;
        }

        RemovePlayer(player);
    }
}