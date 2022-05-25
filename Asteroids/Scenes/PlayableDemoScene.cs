using System.Numerics;
using Asteroids.Behaviors;
using Asteroids.Components;
using Asteroids.Engine;
using Asteroids.Entities;
using Asteroids.Rendering;
using Asteroids.Utils;

namespace Asteroids.Scenes;

public class PlayableDemoScene : Scene
{
    private readonly Spawner _spawner;
    private readonly Vars _vars;
    private readonly CameraController _cameraController;
    private readonly BehaviorController _behaviorController;

    public PlayableDemoScene(Spawner spawner, Vars vars, CameraController cameraController, BehaviorController behaviorController)
    {
        _spawner = spawner;
        _vars = vars;
        _cameraController = cameraController;
        _behaviorController = behaviorController;
    }

    public override string Name
    {
        get => Constants.Scenes.PlayableDemo;
    }

    public override void Load()
    {
        const float radius = 10.0f;
        
        _vars.SetVar(Constants.Vars.Engine_TimeMultiplier, 1.0f);

        CollisionBehavior collisionBehavior = new CollisionBehavior();
        AsteroidSpawnBehavior asteroidSpawnBehavior = new AsteroidSpawnBehavior(_spawner);
        collisionBehavior.CollisionDetected += asteroidSpawnBehavior.HandleCollision;
        _behaviorController.AddBehavior(collisionBehavior);
        _behaviorController.AddBehavior(asteroidSpawnBehavior);
        _behaviorController.AddBehavior(new MovementBehavior());

        Spaceship spaceship = _spawner.SpawnSpaceship(new Vector2(+2.5f, -2.0f), Constants.Colors.Green);
        spaceship.AddComponent(new SpaceshipControlComponent());
        _cameraController.CurrentCamera = new Camera(spaceship);
        _behaviorController.AddBehavior(new PlayerBehavior(spaceship));

        _spawner.SpawnAsteroid(new Vector2(-5.0f, 0f), new Vector2(1.0f, 0.0f), scale: 1.0f);

        collisionBehavior.CollisionDetected += (_, _, _) =>
        {
            float rightAngle = Random.Shared.NextSingle() * MathF.Tau;
            
            _spawner.SpawnAsteroid(
                MathUtils.FromPolar(rightAngle, radius),
                MathUtils.FromPolar(rightAngle + MathF.PI, radius));
        };
    }
}