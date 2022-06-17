using System.Numerics;

namespace Asteroids;

public static class Constants
{
    public static class Scenes
    {
        public const string Testbed = "Testbed";
        public const string AsteroidsDemo = "AsteroidsDemo";
        public const string SpaceshipDemo = "SpaceshipDemo";
        public const string AsteroidCollision = "AsteroidCollision";
        public const string PlayableDemo = "PlayableDemo";
    }

    public static class Colors
    {
        public static readonly Vector3 Red = new Vector3(0.7f, 0.0f, 0.0f);
        public static readonly Vector3 MaxRed = new Vector3(1.0f, 0.0f, 0.0f);
        public static readonly Vector3 Green = new Vector3(0.0f, 0.7f, 0.0f);
        public static readonly Vector3 Blue = new Vector3(0.0f, 0.0f, 0.7f);
        public static readonly Vector3 Yellow = new Vector3(0.7f, 0.7f, 0.0f);
        public static readonly Vector3 Pink = new Vector3(0.7f, 0.0f, 0.7f);
        public static readonly Vector3 Cyan = new Vector3(0.0f, 0.7f, 0.7f);
        public static readonly Vector3 Gray = new Vector3(0.7f, 0.7f, 0.7f);
        public static readonly Vector3 DarkGray = new Vector3(0.3f, 0.3f, 0.3f);
        public static readonly Vector3 White = new Vector3(1.0f, 1.0f, 1.0f);

        public static readonly Vector3[] All =
        {
            Red,
            MaxRed,
            Green,
            Blue,
            Yellow,
            Pink,
            Cyan,
            Gray,
            DarkGray,
            White
        };
    }

    public static class Vars
    {
        public const string DebugEnabled = "Debug:Enabled";
        public const string EngineTimeMultiplier = "Engine:TimeMultiplier";
        public const string PhysicsShowActivationRadius = "Physics:ShowActivationRadius";
        public const string PhysicsShowCollider = "Physics:ShowCollider";
        public const string AsteroidsSpawningRadius = "AsteroidsSpawning:Radius";
        public const string AsteroidsSpawningCooldown = "AsteroidsSpawning:Cooldown";

        public const string ClientUIShowMenu = "ClientUI:ShowMenu";
        public const string ClientUIShowDebug = "ClientUI:ShowDebug";
    }

    public static class Asteroids
    {
        public const int SpikesCount = 8;
    }
}