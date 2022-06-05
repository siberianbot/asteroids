using System.Numerics;
using Asteroids.Components;
using Asteroids.Controllers;
using Asteroids.Engine;
using Asteroids.Entities;
using Asteroids.Physics;

namespace Asteroids.Behaviors;

// TODO:
// 1. spawning of asteroids working incorrectly (incorrect angles, velocities, etc)
// 2. looks bad, refactor

public class CollisionHandlingBehavior : IBehavior
{
    private readonly EntityController _entityController;
    private readonly Spawner _spawner;
    private readonly List<Entity> _excluded = new List<Entity>();
    private readonly List<Collision> _collisions = new List<Collision>();

    public CollisionHandlingBehavior(EntityController entityController, EventQueue eventQueue, Spawner spawner)
    {
        _entityController = entityController;
        _spawner = spawner;

        eventQueue.Subscribe(EventType.CollisionStarted, @event =>
        {
            if (_excluded.Contains(@event.Collision!.Left) || _excluded.Contains(@event.Collision.Right))
            {
                return;
            }

            _collisions.Add(@event.Collision);
        });

        eventQueue.Subscribe(EventType.EntityDestroy, @event => _excluded.Remove(@event.Entity!));

        eventQueue.Subscribe(EventType.CollisionFinished, @event =>
        {
            if (_excluded.Contains(@event.Collision!.Left))
            {
                _excluded.Remove(@event.Collision.Left);
            }

            if (_excluded.Contains(@event.Collision.Right))
            {
                _excluded.Remove(@event.Collision.Right);
            }
        });
    }

    public void Update(float delta)
    {
        foreach (Collision collision in _collisions)
        {
            HandleCollision(collision, delta);
        }

        _collisions.Clear();
    }

    private void HandleCollision(Collision collision, float delta)
    {
        if (collision.Right is Bullet rightBullet && rightBullet.Owner == collision.Left ||
            collision.Left is Bullet leftBullet && leftBullet.Owner == collision.Right)
        {
            return;
        }
        
        MovementComponent leftMovement = collision.Left.GetComponent<MovementComponent>() ?? throw new ArgumentException();
        PositionComponent leftPosition = collision.Left.GetComponent<PositionComponent>() ?? throw new ArgumentException();
        MovementComponent rightMovement = collision.Right.GetComponent<MovementComponent>() ?? throw new ArgumentException();
        PositionComponent rightPosition = collision.Right.GetComponent<PositionComponent>() ?? throw new ArgumentException();

        if (collision.Left is Asteroid leftAsteroid && leftAsteroid.Scale > 0.25f)
        {
            float scale = leftAsteroid.Scale / 2;
            Vector2 deflected = leftMovement.Direction + rightMovement.Direction;
            float velocity = deflected.Length();
            Vector2 position = leftPosition.Position + leftMovement.Direction * delta;

            deflected = Vector2.Normalize(deflected);

            _excluded.Add(_spawner.SpawnAsteroid(position, deflected, velocity, scale));
            _excluded.Add(_spawner.SpawnAsteroid(position,
                Vector2.Reflect(
                    deflected,
                    Vector2.Normalize(new Vector2(-leftMovement.Direction.Y, leftMovement.Direction.X))),
                velocity, scale)
            );
        }

        if (collision.Right is Asteroid rightAsteroid && rightAsteroid.Scale > 0.25f)
        {
            float scale = rightAsteroid.Scale / 2;
            Vector2 deflected = leftMovement.Direction + rightMovement.Direction;
            float velocity = deflected.Length();
            Vector2 position = rightPosition.Position + rightMovement.Direction * delta;

            deflected = Vector2.Normalize(deflected);

            _excluded.Add(_spawner.SpawnAsteroid(position, Vector2.Normalize(deflected), velocity, scale));
            _excluded.Add(_spawner.SpawnAsteroid(position,
                Vector2.Reflect(
                    deflected,
                    Vector2.Normalize(new Vector2(-rightMovement.Direction.Y, rightMovement.Direction.X))),
                velocity, scale));
        }

        _entityController.DestroyEntity(collision.Left);
        _entityController.DestroyEntity(collision.Right);
    }
}