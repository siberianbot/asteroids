using Asteroids.Entities;

namespace Asteroids.Server;

public interface IClient
{
    Player? Player { get; set; }

    Camera? Camera { get; set; }

    string Name { get; }
}