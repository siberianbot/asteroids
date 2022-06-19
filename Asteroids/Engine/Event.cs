using Asteroids.Entities;
using Asteroids.Input;
using Asteroids.Physics;
using Asteroids.Server;

namespace Asteroids.Engine;

public struct Event
{
    public EventType EventType { get; init; }

    public Entity? Entity { get; init; }

    public Collision? Collision { get; init; }

    public string? SceneName { get; init; }

    public IClient? Client { get; init; }

    public string? ClientName { get; init; }

    public ClientAction? ClientAction { get; init; }

    public ClientActionState? ClientActionState { get; init; }
}