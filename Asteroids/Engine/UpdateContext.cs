using Asteroids.Controllers;
using Asteroids.Entities;

namespace Asteroids.Engine;

// TODO: UpdateContext should be lightweight
public struct UpdateContext
{
    public float Delta { get; init; }

    public EngineVars EngineVars { get; init; }

    public CommandQueue CommandQueue { get; init; }

    public EventQueue EventQueue { get; init; }

    public Spawner Spawner { get; init; }

    public Vars GlobalVars { get; init; }

    public ControllersCollection Controllers { get; init; }
}