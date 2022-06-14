using Asteroids.Entities;

namespace Asteroids.Server;

public interface IClient
{
    Player? Player { get; set; }

    string Name { get; }
}