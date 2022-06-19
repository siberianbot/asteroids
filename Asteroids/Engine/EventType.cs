namespace Asteroids.Engine;

public enum EventType
{
    ServerConnected,
    ServerDisconnected,

    ClientConnected,
    ClientDisconnected,
    ClientAction,

    CollisionStarted,
    CollisionFinished,
    SceneChange,

    EntitySpawn,
    EntityDestroy,
}