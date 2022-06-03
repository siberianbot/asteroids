using System.Numerics;

namespace Asteroids.Engine;

public class EngineVars
{
    public double UpdateTimeMs { get; set; }

    public double RenderTimeMs { get; set; }

    public Vector2 ScreenDimensions { get; set; }

    public float ScreenAspectRatio
    {
        get => ScreenDimensions.X / ScreenDimensions.Y;
    }
}