namespace Asteroids.Server;

public interface IServer
{
    void Start();

    void Stop();

    ServerState State { get; }
}