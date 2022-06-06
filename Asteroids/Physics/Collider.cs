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

    public static Collider Translate(Collider collider, Vector2 position, float angle)
    {
        Vector2 origin = Vector2.Zero;

        return new Collider(
            MathUtils.Rotate(collider.A, angle, origin) + position,
            MathUtils.Rotate(collider.B, angle, origin) + position,
            MathUtils.Rotate(collider.C, angle, origin) + position
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

    public static IEnumerable<float> DataOf(Collider collider)
    {
        yield return collider.A.X;
        yield return collider.A.Y;
        yield return collider.B.X;
        yield return collider.B.Y;
        yield return collider.C.X;
        yield return collider.C.Y;
    }
}