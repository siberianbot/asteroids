using System.Numerics;
using Asteroids.Physics;
using Asteroids.Utils;
using Silk.NET.Maths;

namespace Asteroids.Components;

public class ColliderComponent : Component
{
    private readonly Lazy<PositionComponent> _positionComponent;
    private readonly IReadOnlyCollection<Collider> _colliders;

    public ColliderComponent(IReadOnlyCollection<Collider> colliders)
    {
        _colliders = colliders;
        _positionComponent = new Lazy<PositionComponent>(() => Owner.GetComponent<PositionComponent>() ?? throw new ArgumentException());
    }

    public IReadOnlyCollection<Collider> Colliders
    {
        get => _colliders
            .Select(collider => Collider.Rotate(collider, Vector2.Zero, _positionComponent.Value.Rotation))
            .Select(collider => Collider.Translate(collider, _positionComponent.Value.Position))
            .ToArray();
    }

    public Box2D<float> BoundingBox
    {
        get => Box2DUtils.FromVerticesCloud(Colliders.SelectMany(Collider.VerticesOf));
    }
}