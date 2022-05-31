using Asteroids.Commands;
using Asteroids.Entities;

namespace Asteroids.Engine;

public class PlayerController
{
    private readonly CommandQueue _commandQueue;
    private readonly List<Player> _players = new List<Player>();

    public PlayerController(CommandQueue commandQueue)
    {
        _commandQueue = commandQueue;
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

    public void Clear()
    {
        _players.Clear();
    }
}