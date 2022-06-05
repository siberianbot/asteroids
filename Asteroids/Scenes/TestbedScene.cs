using System.Numerics;
using Asteroids.Behaviors;
using Asteroids.Components;
using Asteroids.Controllers;
using Asteroids.Entities;

namespace Asteroids.Scenes;

public class TestbedScene : Scene
{
    private readonly Spawner _spawner;
    private readonly BehaviorFactory _behaviorFactory;
    private readonly CameraController _cameraController;
    private readonly BehaviorController _behaviorController;

    public TestbedScene(Spawner spawner, BehaviorFactory behaviorFactory, ControllersCollection controllersCollection)
    {
        _spawner = spawner;
        _behaviorFactory = behaviorFactory;
        _cameraController = controllersCollection.GetController<CameraController>();
        _behaviorController = controllersCollection.GetController<BehaviorController>();
    }

    public override string Name
    {
        get => Constants.Scenes.Testbed;
    }

    public override void Load()
    {
        Player player = _spawner.SpawnPlayer("test player", Constants.Colors.Green);
        Spaceship spaceship = _spawner.SpawnSpaceship(new Vector2(+2.5f, -2.0f), player);
        spaceship.AddComponent(new SpaceshipControlComponent());

        _cameraController.CurrentCamera = _spawner.SpawnCamera(spaceship);
        _behaviorController.AddBehavior(_behaviorFactory.CreatePlayerControlBehavior());
        _behaviorController.AddBehavior(_behaviorFactory.CreateMovementBehavior());
        _behaviorController.AddBehavior(_behaviorFactory.CreateEntityCleanupBehavior());
        _spawner.SpawnAsteroid(Vector2.Zero, Vector2.Zero, scale: 1.0f);
        _spawner.SpawnAsteroid(new Vector2(-2.5f, 0f), Vector2.Zero, scale: 0.75f);
        _spawner.SpawnAsteroid(new Vector2(+2.5f, 0f), Vector2.Zero, scale: 0.5f);
        _spawner.SpawnAsteroid(new Vector2(-5.0f, 0f), new Vector2(1.0f, 0.0f), scale: 0.25f);
        _spawner.SpawnBullet(spaceship, new Vector2(-2.5f, -2.0f), new Vector2(0.0f, 0.0f));
    }
}