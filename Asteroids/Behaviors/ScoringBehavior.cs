using Asteroids.Entities;
using Asteroids.Physics;

namespace Asteroids.Behaviors;

public class ScoringBehavior : IBehavior
{
    private readonly List<Collision> _collisions = new List<Collision>();

    public ScoringBehavior(CollisionDetectionBehavior collisionDetectionBehavior)
    {
        collisionDetectionBehavior.CollisionStarted += collision =>
        {
            if (collision.Left is Bullet ^ collision.Right is Bullet)
            {
                _collisions.Add(collision);
            }
        };
    }

    public void Update(UpdateContext context)
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

    private void HandleCollision(Collision collision)
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