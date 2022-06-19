using Asteroids.Engine;
using Asteroids.Entities;

namespace Asteroids.Server;

public interface IServer
{
    void Start();

    void Stop();

    void Push(Event @event);

    ServerState State { get; }

    IEntityCollection? EntityCollection { get; }

    IEnumerable<IClient> Clients { get; }
}