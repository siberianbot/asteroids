using System.Numerics;
using Silk.NET.Maths;

namespace Asteroids.Utils;

public static class MathUtils
{
    public static float NormalizeRadian(float radian)
    {
        if (radian < 0)
        {
            while (radian < 0)
            {
                radian += MathF.Tau;
            }
        }
        else
        {
            while (radian > 0)
            {
                radian -= MathF.Tau;
            }
        }

        return radian;
    }

    public static Vector2 FromPolar(float angle, float radius)
    {
        return new Vector2(
            radius * MathF.Cos(angle),
            radius * MathF.Sin(angle)
        );
    }

    public static float GetAngle(Vector2 vector)
    {
        if (vector.X > 0 && vector.Y >= 0)
        {
            return MathF.Atan(vector.Y / vector.X);
        }

        if (vector.X > 0 && vector.Y < 0)
        {
            return MathF.Atan(vector.Y / vector.X) + MathF.Tau;
        }

        if (vector.X < 0)
        {
            return MathF.Atan(vector.Y / vector.X) + MathF.PI;
        }

        if (vector.X == 0 && vector.Y > 0)
        {
            return MathF.PI / 2;
        }

        if (vector.X == 0 && vector.Y < 0)
        {
            return 3 * MathF.PI / 2;
        }

        throw new ArgumentOutOfRangeException();
    }

    public static Vector2 Rotate(Vector2 v, float r)
    {
        float cos = MathF.Cos(r);
        float sin = MathF.Sin(r);

        return new Vector2(v.X * cos - v.Y * sin, v.X * sin + v.Y * cos);
    }

    public static bool Intersects(Box2D<float> left, Box2D<float> right)
    {
        Vector2D<float> maxDistance = left.Size / 2 + right.Size / 2;
        Vector2D<float> diff = right.Center - left.Center;

        return MathF.Abs(diff.X) <= maxDistance.X && MathF.Abs(diff.Y) <= maxDistance.Y;
    }

    public static float CrossProduct(Vector2 left, Vector2 right)
    {
        return left.X * right.Y - right.X * left.Y;
    }

    public static bool IsIntersecting(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
    {
        float t = (
            (a1.X - b1.X) * (b1.Y - b2.Y) - (a1.Y - b1.Y) * (b1.X - b2.X)
        ) / (
            (a1.X - a2.X) * (b1.Y - b2.Y) - (a1.Y - a2.Y) * (b1.X - b2.X)
        );

        float u = (
            (a1.X - b1.X) * (a1.Y - a2.Y) - (a1.Y - b1.Y) * (a1.X - a2.X)
        ) / (
            (a1.X - a2.X) * (b1.Y - b2.Y) - (a1.Y - a2.Y) * (b1.X - b2.X)
        );

        return 0 <= t && t <= 1 &&
               0 <= u && u <= 1;
    }
}