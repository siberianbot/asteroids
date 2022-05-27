using System.Numerics;
using Asteroids.Utils;
using Silk.NET.Maths;

namespace Asteroids.Physics;

public static class CollisionDetector
{
    public static bool BoundingBoxCollisionTest(Box2D<float> left, Box2D<float> right)
    {
        Vector2D<float> maxDistance = left.Size / 2 + right.Size / 2;
        Vector2D<float> diff = right.Center - left.Center;

        return MathF.Abs(diff.X) <= maxDistance.X && MathF.Abs(diff.Y) <= maxDistance.Y;
    }

    public static bool CollidersCollisionTest(Collider left, Collider right)
    {
        if (Collider.VerticesOf(left).Any(leftVertex => PointColliderCollisionTest(right, leftVertex)))
        {
            return true;
        }

        if (Collider.VerticesOf(right).Any(rightVertex => PointColliderCollisionTest(left, rightVertex)))
        {
            return true;
        }

        foreach ((Vector2 leftA, Vector2 leftB) in Collider.EdgesOf(left))
        {
            foreach ((Vector2 rightA, Vector2 rightB) in Collider.EdgesOf(right))
            {
                if (LineSegmentCollisionTest(leftA, leftB, rightA, rightB))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static bool PointColliderCollisionTest(Collider c, Vector2 p)
    {
        // http://totologic.blogspot.com/2014/01/accurate-point-in-triangle-test.html
        // 2nd method : parametric equations system

        float denominator = c.A.X * (c.B.Y - c.C.Y) + c.A.Y * (c.C.X - c.B.X) + c.B.X * c.C.Y - c.B.Y * c.C.X;

        float t1 = (
            p.X * (c.C.Y - c.A.Y) + p.Y * (c.A.X - c.C.X) - c.A.X * c.C.Y + c.A.Y * c.C.X
        ) / denominator;

        float t2 = (
            p.X * (c.B.Y - c.A.Y) + p.Y * (c.A.X - c.B.X) - c.A.X * c.B.Y + c.A.Y * c.B.X
        ) / -denominator;

        return 0 <= t1 && t1 <= 1 &&
               0 <= t2 && t2 <= 1 &&
               t1 + t2 <= 1;
    }

    public static bool LineSegmentCollisionTest(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
    {
        // https://stackoverflow.com/a/565282

        Vector2 r = a2 - a1;
        Vector2 s = b2 - b1;

        float numerator = MathUtils.CrossProduct(b1 - a1, r);
        float denominator = MathUtils.CrossProduct(r, s);

        if (denominator != 0)
        {
            float t = MathUtils.CrossProduct(b1 - a1, s) / denominator;
            float u = numerator / denominator;

            return 0 <= t && t <= 1 &&
                   0 <= u && u <= 1;
        }

        if (numerator == 0)
        {
            float t0 = Vector2.Dot(b1 - a1, r) / Vector2.Dot(r, r);
            float t1 = t0 + Vector2.Dot(s, r) / Vector2.Dot(r, r);

            float absT0 = MathF.Abs(t0);
            float absT1 = MathF.Abs(t1);

            return 0 <= absT0 && absT0 <= 1 &&
                   0 <= absT1 && absT1 <= 1;
        }

        return false;
    }
}