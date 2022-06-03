using Asteroids.Controllers;
using Asteroids.Entities;

namespace Asteroids.Engine;

// TODO: UpdateContext should be lightweight
public struct UpdateContext
{
    public float Delta { get; init; }

    public EngineVars EngineVars { get; init; }

    public CommandQueue CommandQueue { get; init; }

    public Spawner Spawner { get; init; }

    public PlayerController PlayerController { get; init; }

    public EntityController EntityController { get; init; }

    public InputController InputController { get; init; }

    public Vars GlobalVars { get; init; }

    public SceneController SceneController { get; init; }

    public CameraController CameraController { get; init; }
}