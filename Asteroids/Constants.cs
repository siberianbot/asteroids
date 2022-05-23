using System.Numerics;

namespace Asteroids;

public static class Constants
{
    public const int AsteroidSpikesCount = 8;

    public const string Testbed = "Testbed";
    public const string AsteroidsDemo = "AsteroidsDemo";
    public const string SpaceshipDemo = "SpaceshipDemo";

    public static readonly Vector3[] Colors =
    {
        new Vector3(0.7f, 0.0f, 0.0f), // Red
        new Vector3(0.0f, 0.7f, 0.0f), // Green
        new Vector3(0.0f, 0.0f, 0.7f), // Blue
        new Vector3(0.7f, 0.7f, 0.0f), // Yellow
        new Vector3(0.7f, 0.0f, 0.7f), // Pink
        new Vector3(0.0f, 0.7f, 0.7f), // Cyan
    };
}