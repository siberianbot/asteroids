using Asteroids.Behaviors;
using Asteroids.Components;
using Asteroids.Engine;
using Asteroids.Entities;
using Asteroids.Utils;

namespace Asteroids.Scenes;

public class AsteroidCollisionScene : Scene
{
    private readonly Spawner _spawner;
    private readonly BehaviorController _behaviorController;
    private readonly Vars _vars;

    public AsteroidCollisionScene(Spawner spawner, BehaviorController behaviorController, Vars vars)
    {
        _spawner = spawner;
        _behaviorController = behaviorController;
        _vars = vars;
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

        CollisionBehavior collisionBehavior = new CollisionBehavior();
        AsteroidSpawnBehavior asteroidSpawnBehavior = new AsteroidSpawnBehavior(_spawner);
        collisionBehavior.CollisionDetected += asteroidSpawnBehavior.HandleCollision;
        _behaviorController.AddBehavior(collisionBehavior);
        _behaviorController.AddBehavior(asteroidSpawnBehavior);
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