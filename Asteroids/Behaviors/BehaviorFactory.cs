using Asteroids.Controllers;
using Asteroids.Engine;
using Asteroids.Entities;

namespace Asteroids.Behaviors;

public class BehaviorFactory
{
    private readonly ControllersCollection _controllersCollection;
    private readonly EngineVars _engineVars;
    private readonly EventQueue _eventQueue;
    private readonly Spawner _spawner;
    private readonly Vars _vars;

    public BehaviorFactory(
        ControllersCollection controllersCollection,
        EngineVars engineVars,
        EventQueue eventQueue,
        Spawner spawner,
        Vars vars)
    {
        _controllersCollection = controllersCollection;
        _engineVars = engineVars;
        _eventQueue = eventQueue;
        _spawner = spawner;
        _vars = vars;
    }

    public AsteroidSpawningBehavior CreateAsteroidSpawningBehavior(float radius, float maxCooldown)
    {
        return new AsteroidSpawningBehavior(
            _controllersCollection.GetController<PlayerController>(),
            _controllersCollection.GetController<EntityController>(),
            radius, maxCooldown);
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

    public DebugBehavior CreateDebugBehavior()
    {
        return new DebugBehavior(
            _eventQueue,
            _engineVars,
            _vars,
            _controllersCollection.GetController<SceneController>());
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
            _controllersCollection.GetController<CameraController>(),
            _controllersCollection.GetController<PlayerController>(),
            _spawner);
    }

    public ScoringBehavior CreateScoringBehavior()
    {
        return new ScoringBehavior(
            _eventQueue);
    }

    public UIBehavior CreateUIBehavior()
    {
        return new UIBehavior(
            _engineVars,
            _controllersCollection.GetController<PlayerController>());
    }
}