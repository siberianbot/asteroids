using Asteroids.Entities;

namespace Asteroids.Server;

public class LocalClient : IClient
{
    public Player? Player { get; set; }

    public string Name { get; init; }
}