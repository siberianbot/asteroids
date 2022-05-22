namespace Asteroids;

public static class Program
{
    public static void Main()
    {
        using Engine.Engine engine = new Engine.Engine();

        engine.Run();
    }
}