using Asteroids.Components;
using Asteroids.Entities;
using Asteroids.Physics;
using Silk.NET.Maths;

namespace Asteroids.Behaviors;

public class CollisionDetectionBehavior : IBehavior
{
    public event Action<Collision> CollisionStarted = delegate { };

    public event Action<Collision> CollisionFinished = delegate { };

    private readonly List<Collision> _activeCollisions = new List<Collision>();

    public void Update(UpdateContext context)
    {
        var checkList = context.DependencyContainer.EntityController.Entities
            .Select(entity => new
            {
                Entity = entity,
                ColliderComponent = entity.GetComponent<ColliderComponent>()
            })
            .Where(entity => entity.ColliderComponent != null)
            .Select(x => new
            {
                x.Entity,
                x.ColliderComponent!.Colliders,
                x.ColliderComponent!.BoundingBox
            })
            .OrderBy(x => x.BoundingBox.Center.X)
            .ToArray();

        for (int i = 0; i < checkList.Length - 1; i++)
        {
            var left = checkList[i];
            var right = checkList[i + 1];
            Collision? collision = GetCollision(left.Entity, right.Entity);

            if (!CollisionTest(left.BoundingBox, right.BoundingBox, left.Colliders, right.Colliders))
            {
                if (collision != null)
                {
                    CollisionFinished.Invoke(collision);
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
            CollisionStarted.Invoke(collision);
        }
    }

    private Collision? GetCollision(Entity left, Entity right)
    {
        return _activeCollisions.FirstOrDefault(collision => collision.Left == left && collision.Right == right ||
                                                             collision.Right == left && collision.Left == right);
    }

    private static bool CollisionTest(Box2D<float> leftBoundingBox, Box2D<float> rightBoundingBox,
        IReadOnlyCollection<Collider> leftColliders, IReadOnlyCollection<Collider> rightColliders)
    {
        if (!CollisionDetector.BoundingBoxCollisionTest(leftBoundingBox, rightBoundingBox))
        {
            return false;
        }

        foreach (Collider leftCollider in leftColliders)
        {
            foreach (Collider rightCollider in rightColliders)
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