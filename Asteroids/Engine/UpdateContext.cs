using Asteroids.Controllers;
using Asteroids.Entities;

namespace Asteroids.Engine;

// TODO: UpdateContext should be lightweight
public struct UpdateContext
{
    public float Delta { get; init; }

    [Obsolete]
    public CommandQueue CommandQueue { get; init; }

    [Obsolete]
    public Spawner Spawner { get; init; }
}