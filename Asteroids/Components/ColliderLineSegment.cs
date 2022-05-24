using System.Numerics;

namespace Asteroids.Components;

public struct ColliderLineSegment
{
    public Vector2 Start { get; }

    public Vector2 End { get; }

    public ColliderLineSegment(Vector2 start, Vector2 end)
    {
        Start = start;
        End = end;
    }
}