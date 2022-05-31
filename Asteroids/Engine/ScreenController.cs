using System.Numerics;

namespace Asteroids.Engine;

public class ScreenController
{
    public Vector2 ScreenDimensions { get; set; }

    public float ScreenAspectRatio
    {
        get => ScreenDimensions.X / ScreenDimensions.Y;
    }
}