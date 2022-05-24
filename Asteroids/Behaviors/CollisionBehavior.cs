using System.Numerics;
using Asteroids.Components;
using Asteroids.Entities;
using Asteroids.Utils;
using Silk.NET.Maths;

namespace Asteroids.Behaviors;

public class CollisionBehavior : IBehavior
{
    private struct Collider
    {
        public Entity Entity { get; init; }

        public Box2D<float> BoundingBox { get; init; }

        public ColliderLineSegment[] Segments { get; init; }

        public Vector2 Position { get; init; }

        public float Rotation { get; init; }
    }

    public event Action<Entity, Entity, Vector2> CollisionDetected = delegate { };

    public void Update(UpdateContext context)
    {
        List<Collider> colliders = new List<Collider>();

        context.DependencyContainer.EntityController.ForEachEntity(entity =>
        {
            ColliderComponent? colliderComponent = entity.GetComponent<ColliderComponent>();

            if (colliderComponent == null)
            {
                return;
            }

            PositionComponent positionComponent = entity.GetComponent<PositionComponent>() ?? throw new ArgumentException();

            colliders.Add(new Collider
            {
                Entity = entity,
                BoundingBox = colliderComponent.BoundingBox,
                Segments = colliderComponent.Segments,
                Position = positionComponent.Position,
                Rotation = positionComponent.Rotation
            });
        });

        colliders = colliders
            .OrderBy(collider => collider.BoundingBox.Center.X)
            .ToList();

        for (int i = 0; i < colliders.Count - 1; i++)
        {
            Collider thisCollider = colliders[i];
            Collider nextCollider = colliders[i + 1];

            Vector2? intersection = CheckIntersection(thisCollider, nextCollider);

            if (intersection == null)
            {
                continue;
            }

            CollisionDetected.Invoke(thisCollider.Entity, nextCollider.Entity, intersection.Value);

            context.DependencyContainer.EntityController.Destroy(thisCollider.Entity);
            context.DependencyContainer.EntityController.Destroy(nextCollider.Entity);
        }
    }

    private static Vector2? CheckIntersection(Collider left, Collider right)
    {
        if (!MathUtils.Intersects(
                left.BoundingBox.GetTranslated(left.Position.ToGeneric()),
                right.BoundingBox.GetTranslated(right.Position.ToGeneric())
            ))
        {
            return null;
        }

        foreach (ColliderLineSegment leftSegment in left.Segments)
        {
            foreach (ColliderLineSegment rightSegment in right.Segments)
            {
                Vector2? intersection = MathUtils.GetIntersection(
                    left.Position + MathUtils.Rotate(leftSegment.Start, left.Rotation),
                    left.Position + MathUtils.Rotate(leftSegment.End, left.Rotation),
                    right.Position + MathUtils.Rotate(rightSegment.Start, right.Rotation),
                    right.Position + MathUtils.Rotate(rightSegment.End, right.Rotation)
                );

                if (intersection != null)
                {
                    return intersection.Value;
                }
            }
        }

        return null;
    }
}