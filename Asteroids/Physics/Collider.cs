using System.Numerics;
using Asteroids.Utils;

namespace Asteroids.Physics;

public readonly struct Collider
{
    public Collider(Vector2 a, Vector2 b, Vector2 c)
    {
        A = a;
        B = b;
        C = c;
    }

    public Vector2 A { get; }
    public Vector2 B { get; }
    public Vector2 C { get; }

    public override string ToString()
    {
        return $"({A}, {B}, {C})";
    }

    public static Collider Translate(Collider collider, Vector2 position)
    {
        return new Collider(
            collider.A + position,
            collider.B + position,
            collider.C + position
        );
    }

    public static Collider Rotate(Collider collider, Vector2 origin, float angle)
    {
        return new Collider(
            MathUtils.Rotate(collider.A, angle, origin),
            MathUtils.Rotate(collider.B, angle, origin),
            MathUtils.Rotate(collider.C, angle, origin)
        );
    }

    public static IEnumerable<Vector2> VerticesOf(Collider collider)
    {
        yield return collider.A;
        yield return collider.B;
        yield return collider.C;
    }

    public static IEnumerable<(Vector2, Vector2)> EdgesOf(Collider collider)
    {
        yield return (collider.A, collider.B);
        yield return (collider.B, collider.C);
        yield return (collider.C, collider.A);
    }
}