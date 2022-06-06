using System.Numerics;
using Asteroids.Components;
using Asteroids.Controllers;
using Asteroids.Engine;
using Asteroids.Entities;
using Asteroids.Physics;

namespace Asteroids.Behaviors;

public class CollisionDetectionBehavior : IBehavior
{
    private struct CollisionDescriptor
    {
        public Entity Entity { get; init; }

        public Vector2 Position { get; init; }

        public float Rotation { get; init; }

        public IEnumerable<Collider> Colliders { get; init; }

        public float Radius { get; init; }
    }

    private readonly EntityController _entityController;
    private readonly EventQueue _eventQueue;
    private readonly List<Collision> _activeCollisions = new List<Collision>();

    public CollisionDetectionBehavior(EntityController entityController, EventQueue eventQueue)
    {
        _entityController = entityController;
        _eventQueue = eventQueue;
    }

    public void Update(float delta)
    {
        CollisionDescriptor[] checkList = _entityController.Entities
            .Select(entity => new
            {
                Entity = entity,
                PositionComponent = entity.GetComponent<PositionComponent>(),
                ColliderComponent = entity.GetComponent<ColliderComponent>()
            })
            .Where(entity => entity.PositionComponent != null && entity.ColliderComponent != null)
            .Select(x => new CollisionDescriptor
            {
                Entity = x.Entity,
                Position = x.PositionComponent!.Position,
                Rotation = x.PositionComponent!.Rotation,
                Colliders = x.ColliderComponent!.Colliders,
                Radius = x.ColliderComponent!.Radius
            })
            .OrderBy(x => x.Position.X)
            .ToArray();

        for (int i = 0; i < checkList.Length - 1; i++)
        {
            CollisionDescriptor left = checkList[i];
            CollisionDescriptor right = checkList[i + 1];
            Collision? collision = GetCollision(left.Entity, right.Entity);

            if (!CollisionTest(left, right))
            {
                if (collision != null)
                {
                    _eventQueue.Push(new Event
                    {
                        EventType = EventType.CollisionFinished,
                        Collision = collision
                    });

                    _activeCollisions.Remove(collision);
                }

                continue;
            }

            if (collision != null)
            {
                continue;
            }

            collision = new Collision(left.Entity, right.Entity);
            _activeCollisions.Add(collision);

            _eventQueue.Push(new Event
            {
                EventType = EventType.CollisionStarted,
                Collision = collision
            });
        }
    }

    private Collision? GetCollision(Entity left, Entity right)
    {
        return _activeCollisions.FirstOrDefault(collision => collision.Left == left && collision.Right == right ||
                                                             collision.Right == left && collision.Left == right);
    }

    private static bool CollisionTest(CollisionDescriptor left, CollisionDescriptor right)
    {
        if (!CollisionDetector.CirclesCollisionTest(left.Position, left.Radius, right.Position, right.Radius))
        {
            return false;
        }

        foreach (Collider leftCollider in left.Colliders.Select(c => Collider.Translate(c, left.Position, left.Rotation)))
        {
            foreach (Collider rightCollider in right.Colliders.Select(c => Collider.Translate(c, right.Position, right.Rotation)))
            {
                if (CollisionDetector.CollidersCollisionTest(leftCollider, rightCollider))
                {
                    return true;
                }
            }
        }

        return false;
    }
}