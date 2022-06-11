using Asteroids.UI;

namespace Asteroids.Server;

public interface IClient
{
    UIContainer ClientUIContainer { get; }
}