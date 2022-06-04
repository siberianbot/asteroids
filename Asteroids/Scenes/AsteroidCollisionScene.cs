using Asteroids.Behaviors;
using Asteroids.Controllers;
using Asteroids.Engine;
using Asteroids.Entities;
using Asteroids.Utils;

namespace Asteroids.Scenes;

public class AsteroidCollisionScene : Scene
{
    private readonly Spawner _spawner;
    private readonly BehaviorController _behaviorController;
    private readonly Vars _vars;
    private readonly EventQueue _eventQueue;

    public AsteroidCollisionScene(Spawner spawner, BehaviorController behaviorController, Vars vars, EventQueue eventQueue)
    {
        _spawner = spawner;
        _behaviorController = behaviorController;
        _vars = vars;
        _eventQueue = eventQueue;
    }

    public override string Name
    {
        get => Constants.Scenes.AsteroidCollision;
    }

    public override void Load()
    {
        _vars.SetVar(Constants.Vars.Engine_TimeMultiplier, 1.0f);

        const float velocity = 0.5f;
        const float radius = 5.0f;

        CollisionDetectionBehavior collisionDetectionBehavior = new CollisionDetectionBehavior();
        _behaviorController.AddBehavior(collisionDetectionBehavior);
        _behaviorController.AddBehavior(new CollisionHandlingBehavior(_eventQueue));
        _behaviorController.AddBehavior(new MovementBehavior());

        float leftAngle = Random.Shared.NextSingle() * MathF.PI + MathF.PI / 2;
        _spawner.SpawnAsteroid(
            MathUtils.FromPolar(leftAngle, radius),
            MathUtils.FromPolar(leftAngle - MathF.PI, radius),
            velocity: velocity);

        float rightAngle = Random.Shared.NextSingle() * MathF.PI - MathF.PI / 2;
        _spawner.SpawnAsteroid(
            MathUtils.FromPolar(rightAngle, radius),
            MathUtils.FromPolar(rightAngle + MathF.PI, radius),
            velocity: velocity);
    }
}