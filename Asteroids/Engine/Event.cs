using Asteroids.Entities;

namespace Asteroids.Engine;

public struct Event
{
    public EventType EventType { get; init; }

    public Entity? Entity { get; init; }
}