using Asteroids.Controllers;
using Asteroids.Engine;
using Asteroids.Entities;

namespace Asteroids.Behaviors;

public class BehaviorFactory
{
    private readonly ControllersCollection _controllersCollection;
    private readonly CommandQueue _commandQueue;
    private readonly EventQueue _eventQueue;
    private readonly Spawner _spawner;
    private readonly Vars _vars;

    public BehaviorFactory(
        ControllersCollection controllersCollection,
        CommandQueue commandQueue,
        EventQueue eventQueue,
        Spawner spawner,
        Vars vars)
    {
        _controllersCollection = controllersCollection;
        _commandQueue = commandQueue;
        _eventQueue = eventQueue;
        _spawner = spawner;
        _vars = vars;
    }

    public AsteroidSpawningBehavior CreateAsteroidSpawningBehavior()
    {
        return new AsteroidSpawningBehavior(
            _controllersCollection.GetController<PlayerController>(),
            _controllersCollection.GetController<EntityController>(),
            _spawner,
            _vars);
    }

    public BulletSpawningBehavior CreateBulletSpawningBehavior()
    {
        return new BulletSpawningBehavior(
            _commandQueue,
            _controllersCollection.GetController<EntityController>(),
            _spawner);
    }

    public CollisionDetectionBehavior CreateCollisionDetectionBehavior()
    {
        return new CollisionDetectionBehavior(
            _controllersCollection.GetController<EntityController>(),
            _eventQueue);
    }

    public CollisionHandlingBehavior CreateCollisionHandlingBehavior()
    {
        return new CollisionHandlingBehavior(
            _controllersCollection.GetController<EntityController>(),
            _eventQueue,
            _spawner);
    }

    public EntityCleanupBehavior CreateEntityCleanupBehavior()
    {
        return new EntityCleanupBehavior(
            _controllersCollection.GetController<EntityController>(),
            _controllersCollection.GetController<PlayerController>());
    }

    public MovementBehavior CreateMovementBehavior()
    {
        return new MovementBehavior(
            _controllersCollection.GetController<EntityController>());
    }

    public PlayerControlBehavior CreatePlayerControlBehavior()
    {
        return new PlayerControlBehavior(
            _controllersCollection.GetController<PlayerController>(),
            _controllersCollection.GetController<EntityController>(),
            _eventQueue);
    }

    public PlayerSpawningBehavior CreatePlayerSpawningBehavior()
    {
        return new PlayerSpawningBehavior(
            _controllersCollection.GetController<PlayerController>(),
            _spawner);
    }

    public ScoringBehavior CreateScoringBehavior()
    {
        return new ScoringBehavior(
            _eventQueue);
    }
}