using Asteroids.Behaviors;
using Asteroids.Controllers;
using Asteroids.Entities;
using Asteroids.Utils;

namespace Asteroids.Scenes;

public class AsteroidCollisionScene : Scene
{
    private readonly Spawner _spawner;
    private readonly BehaviorFactory _behaviorFactory;
    private readonly BehaviorController _behaviorController;

    public AsteroidCollisionScene(Spawner spawner, BehaviorFactory behaviorFactory, ControllersCollection controllersCollection)
    {
        _spawner = spawner;
        _behaviorFactory = behaviorFactory;
        _behaviorController = controllersCollection.GetController<BehaviorController>();
    }

    public override string Name
    {
        get => Constants.Scenes.AsteroidCollision;
    }

    public override void Load()
    {
        const float velocity = 0.5f;
        const float radius = 5.0f;

        _behaviorController.AddBehavior(_behaviorFactory.CreateCollisionDetectionBehavior());
        _behaviorController.AddBehavior(_behaviorFactory.CreateCollisionHandlingBehavior());
        _behaviorController.AddBehavior(_behaviorFactory.CreateMovementBehavior());

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