using Asteroids.Engine;
using Asteroids.Entities;
using Asteroids.Physics;

namespace Asteroids.Behaviors;

public class ScoringBehavior : IBehavior
{
    private readonly EventQueue _eventQueue;
    private readonly List<Collision> _collisions = new List<Collision>();
    private long _collisionStartedSubscription;

    public ScoringBehavior(EventQueue eventQueue)
    {
        _eventQueue = eventQueue;
    }

    public void Initialize()
    {
        _collisionStartedSubscription = _eventQueue.Subscribe(EventType.CollisionStarted, @event =>
        {
            if (@event.Collision!.Left is Bullet ^ @event.Collision.Right is Bullet)
            {
                _collisions.Add(@event.Collision);
            }
        });
    }

    public void Terminate()
    {
        _eventQueue.Unsubscribe(EventType.CollisionStarted, _collisionStartedSubscription);
    }

    public void Update(float delta)
    {
        foreach (Collision collision in _collisions)
        {
            HandleCollision(collision);
        }

        _collisions.Clear();
    }

    private enum CollisionSide
    {
        Left,
        Right
    }

    private static void HandleCollision(Collision collision)
    {
        (Player owner, CollisionSide side) = collision.Left is Bullet left && left.Owner is Spaceship leftSpaceship
            ? (leftSpaceship.Owner as Player ?? throw new ArgumentException(), CollisionSide.Left)
            : collision.Right is Bullet right && right.Owner is Spaceship rightSpaceship
                ? (rightSpaceship.Owner as Player ?? throw new ArgumentException(), CollisionSide.Right)
                : throw new ArgumentException();

        Entity target = side switch
        {
            CollisionSide.Left => collision.Right,
            CollisionSide.Right => collision.Left,
            _ => throw new ArgumentException()
        };

        owner.Score += target switch
        {
            Asteroid asteroid => (long)(100L * asteroid.Scale),
            Spaceship => 1000L,
            _ => throw new ArgumentException()
        };
    }
}