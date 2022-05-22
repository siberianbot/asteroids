namespace Asteroids.Utils;

public static class GenerationUtils
{
    public static IEnumerable<uint> GenerateIndicesData(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return (uint)i;
        }
    }
}