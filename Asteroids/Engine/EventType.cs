namespace Asteroids.Engine;

public enum EventType
{
    EngineReady,

    ServerConnected,
    ServerDisconnected,

    ClientConnected,
    ClientDisconnected,
    KeyPress,
    KeyRelease,

    CollisionStarted,
    CollisionFinished,
    EntityDestroy,
    SceneChange,
}