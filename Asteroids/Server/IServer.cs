using Asteroids.Entities;

namespace Asteroids.Server;

public interface IServer
{
    void Start();

    void Stop();

    IClient Join(string name);

    void Leave(IClient client);

    ServerState State { get; }

    IEntityCollection? EntityCollection { get; }

    IPlayerCollection? PlayerCollection { get; }
}