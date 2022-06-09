using Asteroids.Entities;
using Asteroids.Physics;
using Silk.NET.Input;

namespace Asteroids.Engine;

public struct Event
{
    public EventType EventType { get; init; }

    public Entity? Entity { get; init; }

    public Collision? Collision { get; init; }

    public Key? Key { get; init; }
}