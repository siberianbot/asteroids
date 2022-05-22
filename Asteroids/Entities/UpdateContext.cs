using Asteroids.Engine;

namespace Asteroids.Entities;

public struct UpdateContext
{
    public float Delta { get; init; }

    public InputController InputController { get; init; }
}