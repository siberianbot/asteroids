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
}