using System.Numerics;

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

    public static Vector2 Rotate(Vector2 v, float r, Vector2 o)
    {
        return Rotate(v - o, r) + o;
    }

    public static float CrossProduct(Vector2 left, Vector2 right)
    {
        return left.X * right.Y - right.X * left.Y;
    }

    public static Vector2? GetIntersection(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
    {
        float denominator = (a1.X - a2.X) * (b1.Y - b2.Y) - (a1.Y - a2.Y) * (b1.X - b2.X);
        float p = a1.X * a2.Y - a1.Y * a2.X;
        float q = b1.X * b2.Y - b1.Y * b2.X;

        float x = (p * (b1.X - b2.X) - q * (a1.X - a2.X)) / denominator;
        float y = (p * (b1.Y - b2.Y) - q * (a1.Y - a2.Y)) / denominator;

        if (float.IsNaN(x) || float.IsNaN(y))
        {
            return null;
        }

        return new Vector2(x, y);
    }

    public static bool AreOpposite(Vector2 a, Vector2 b)
    {
        return Vector2.Dot(a, b) <= 0;
    }
}