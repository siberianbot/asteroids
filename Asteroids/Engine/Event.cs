using Asteroids.Entities;
using Asteroids.Physics;
using Asteroids.Server;
using Silk.NET.Input;

namespace Asteroids.Engine;

public struct Event
{
    public EventType EventType { get; init; }

    public Entity? Entity { get; init; }

    public Collision? Collision { get; init; }

    public Key? Key { get; init; }

    public string? MenuName { get; init; }

    public string? SceneName { get; init; }

    public IClient? Client { get; init; }
}