namespace Asteroids.Entities;

public interface IPlayerCollection
{
    IReadOnlyCollection<Player> Players { get; }
}