using System.Numerics;
using Asteroids.Behaviors;
using Asteroids.Components;
using Asteroids.Controllers;
using Asteroids.Engine;
using Asteroids.Entities;
using Asteroids.Rendering;

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

        CollisionDetectionBehavior collisionDetectionBehavior = new CollisionDetectionBehavior();
        _behaviorController.AddBehavior(collisionDetectionBehavior);
        _behaviorController.AddBehavior(new CollisionHandlingBehavior(collisionDetectionBehavior));
        _behaviorController.AddBehavior(new ScoringBehavior(collisionDetectionBehavior));
        _behaviorController.AddBehavior(new MovementBehavior());
        _behaviorController.AddBehavior(new AsteroidSpawningBehavior(radius, 5.0f));
        _behaviorController.AddBehavior(new PlayerControlBehavior());
        _behaviorController.AddBehavior(new PlayerSpawningBehavior());
        _behaviorController.AddBehavior(new EntityCleanupBehavior());

        _spawner.SpawnPlayer("Asteroids Player", Constants.Colors.Green);
    }
}