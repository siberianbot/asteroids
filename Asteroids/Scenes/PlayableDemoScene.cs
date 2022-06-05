using Asteroids.Behaviors;
using Asteroids.Controllers;
using Asteroids.Entities;

namespace Asteroids.Scenes;

public class PlayableDemoScene : Scene
{
    private readonly Spawner _spawner;
    private readonly BehaviorFactory _behaviorFactory;
    private readonly BehaviorController _behaviorController;


    public PlayableDemoScene(Spawner spawner, BehaviorFactory behaviorFactory, ControllersCollection controllersCollection)
    {
        _spawner = spawner;
        _behaviorFactory = behaviorFactory;
        _behaviorController = controllersCollection.GetController<BehaviorController>();
    }

    public override string Name
    {
        get => Constants.Scenes.PlayableDemo;
    }

    public override void Load()
    {
        const float radius = 10.0f;
        const float cooldown = 5.0f;

        _behaviorController.AddBehavior(_behaviorFactory.CreateCollisionDetectionBehavior());
        _behaviorController.AddBehavior(_behaviorFactory.CreateCollisionHandlingBehavior());
        _behaviorController.AddBehavior(_behaviorFactory.CreateScoringBehavior());
        _behaviorController.AddBehavior(_behaviorFactory.CreateMovementBehavior());
        _behaviorController.AddBehavior(_behaviorFactory.CreateAsteroidSpawningBehavior(radius, cooldown));
        _behaviorController.AddBehavior(_behaviorFactory.CreatePlayerControlBehavior());
        _behaviorController.AddBehavior(_behaviorFactory.CreatePlayerSpawningBehavior());
        _behaviorController.AddBehavior(_behaviorFactory.CreateEntityCleanupBehavior());

        _spawner.SpawnPlayer("Asteroids Player", Constants.Colors.Green);
    }
}