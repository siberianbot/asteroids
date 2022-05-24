using System.Numerics;
using Asteroids.Utils;
using Silk.NET.Maths;

namespace Asteroids.Components;

public class ColliderComponent : Component
{
    private readonly Lazy<PositionComponent> _positionComponent;

    public ColliderComponent(List<Vector2> points)
    {
        Segments = new ColliderLineSegment[points.Count];

        for (int idx = 0; idx < points.Count; idx++)
        {
            int endPointIdx = idx + 1 == points.Count ? 0 : idx + 1;

            Segments[idx] = new ColliderLineSegment(points[idx], points[endPointIdx]);
        }

        _positionComponent = new Lazy<PositionComponent>(() => Owner.GetComponent<PositionComponent>() ?? throw new ArgumentException());
    }

    public ColliderLineSegment[] Segments { get; }

    public Box2D<float> BoundingBox
    {
        get
        {
            float rotation = _positionComponent.Value.Rotation;

            Vector2[] rotatedPoints = Segments
                .Select(segment => MathUtils.Rotate(segment.Start, rotation))
                .ToArray();

            return new Box2D<float>(
                rotatedPoints.Min(point => point.X),
                rotatedPoints.Min(point => point.Y),
                rotatedPoints.Max(point => point.X),
                rotatedPoints.Max(point => point.Y));
        }
    }
}