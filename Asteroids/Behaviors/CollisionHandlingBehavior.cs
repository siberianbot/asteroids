using System.Numerics;
using Asteroids.Components;
using Asteroids.Controllers;
using Asteroids.Engine;
using Asteroids.Entities;
using Asteroids.Physics;

namespace Asteroids.Behaviors;

public class CollisionHandlingBehavior : IBehavior
{
    private readonly List<Entity> _excluded = new List<Entity>();
    private readonly List<Collision> _collisions = new List<Collision>();

    public CollisionHandlingBehavior(EventQueue eventQueue)
    {
        eventQueue.OnEvent(EventType.CollisionStarted, @event =>
        {
            if (_excluded.Contains(@event.Collision!.Left) || _excluded.Contains(@event.Collision.Right))
            {
                return;
            }

            _collisions.Add(@event.Collision);
        });

        eventQueue.OnEvent(EventType.EntityDestroy, @event => _excluded.Remove(@event.Entity!));

        eventQueue.OnEvent(EventType.CollisionFinished, @event =>
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

    public void Update(UpdateContext context)
    {
        foreach (Collision collision in _collisions)
        {
            HandleCollision(collision, context.Delta, context.Spawner, context.Controllers.GetController<EntityController>());
        }

        _collisions.Clear();
    }

    private void HandleCollision(Collision collision, float delta, Spawner spawner, EntityController entityController)
    {
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

            _excluded.Add(spawner.SpawnAsteroid(position, deflected, velocity, scale));
            _excluded.Add(spawner.SpawnAsteroid(position,
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

            _excluded.Add(spawner.SpawnAsteroid(position, Vector2.Normalize(deflected), velocity, scale));
            _excluded.Add(spawner.SpawnAsteroid(position,
                Vector2.Reflect(
                    deflected,
                    Vector2.Normalize(new Vector2(-rightMovement.Direction.Y, rightMovement.Direction.X))),
                velocity, scale));
        }

        entityController.DestroyEntity(collision.Left);
        entityController.DestroyEntity(collision.Right);
    }
}