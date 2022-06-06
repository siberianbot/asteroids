using Asteroids.Physics;

namespace Asteroids.Components;

public class ColliderComponent : Component
{
    public ColliderComponent(IReadOnlyCollection<Collider> colliders, float radius)
    {
        Colliders = colliders;
        Radius = radius;
    }

    public IReadOnlyCollection<Collider> Colliders { get; }

    public float Radius { get; }
}