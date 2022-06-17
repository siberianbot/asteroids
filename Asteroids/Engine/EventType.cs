namespace Asteroids.Engine;

public enum EventType
{
    EngineReady,

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