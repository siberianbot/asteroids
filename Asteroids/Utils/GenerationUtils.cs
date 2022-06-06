namespace Asteroids.Utils;

public static class GenerationUtils
{
    public static IEnumerable<float> GenerateCircleVerticesData(float radius, int count)
    {
        float step = MathF.Tau / count;

        for (float angle = 0; angle < MathF.Tau; angle += step)
        {
            yield return radius * MathF.Cos(angle);
            yield return radius * MathF.Sin(angle);
        }
    }

    public static IEnumerable<uint> GenerateIndicesData(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return (uint)i;
        }
    }
}