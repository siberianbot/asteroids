using Asteroids.Engine;
using Asteroids.Entities;

namespace Asteroids.Server;

public interface IClient
{
    Player? Player { get; set; }

    Camera? Camera { get; set; }

    string Name { get; }

    bool IsJoined { get; }

    void JoinServer(IServer server);

    void LeaveServer();

    void PushEvent(Event @event);
}