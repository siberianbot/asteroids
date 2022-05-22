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
}