using Asteroids.Entities;

namespace Asteroids.Server;

public class LocalClient : IClient
{
    public Player? Player { get; set; }

    public Camera? Camera { get; set; }

    public string Name { get; init; }
}