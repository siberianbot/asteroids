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
    ShowMenu,
    HideMenu,

    CollisionStarted,
    CollisionFinished,
    EntityDestroy,
    SceneChange,
}