using System.Numerics;
using Asteroids.Behaviors;
using Asteroids.Components;
using Asteroids.Engine;
using Asteroids.Entities;
using Asteroids.Rendering;

namespace Asteroids.Scenes;

public class TestbedScene : Scene
{
    private readonly Spawner _spawner;
    private readonly CameraController _cameraController;
    private readonly BehaviorController _behaviorController;
    private readonly Vars _vars;

    public TestbedScene(Spawner spawner, CameraController cameraController, BehaviorController behaviorController, Vars vars)
    {
        _spawner = spawner;
        _cameraController = cameraController;
        _behaviorController = behaviorController;
        _vars = vars;
    }

    public override string Name
    {
        get => Constants.Scenes.Testbed;
    }

    public override void Load()
    {
        _vars.SetVar(Constants.Vars.Engine_TimeMultiplier, 1.0f);

        Player player = _spawner.SpawnPlayer("test player", Constants.Colors.Green);
        Spaceship spaceship = _spawner.SpawnSpaceship(new Vector2(+2.5f, -2.0f), player);
        spaceship.AddComponent(new SpaceshipControlComponent());

        _cameraController.CurrentCamera = _spawner.SpawnCamera(spaceship);
        _behaviorController.AddBehavior(new PlayerControlBehavior());
        _behaviorController.AddBehavior(new MovementBehavior());
        _spawner.SpawnAsteroid(Vector2.Zero, Vector2.Zero, scale: 1.0f);
        _spawner.SpawnAsteroid(new Vector2(-2.5f, 0f), Vector2.Zero, scale: 0.75f);
        _spawner.SpawnAsteroid(new Vector2(+2.5f, 0f), Vector2.Zero, scale: 0.5f);
        _spawner.SpawnAsteroid(new Vector2(-5.0f, 0f), new Vector2(1.0f, 0.0f), scale: 0.25f);
        _spawner.SpawnBullet(spaceship, new Vector2(-2.5f, -2.0f), new Vector2(0.0f, 0.0f));
    }
}