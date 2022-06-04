using Asteroids.Entities;
using Asteroids.Physics;

namespace Asteroids.Engine;

public struct Event
{
    public EventType EventType { get; init; }

    public Entity? Entity { get; init; }

    public Collision? Collision { get; init; }
}