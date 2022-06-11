using Asteroids.UI;

namespace Asteroids.Server;

public class LocalClient : IClient
{
    public LocalClient(Engine.Engine engine)
    {
        
    }
    
    public UIContainer ClientUIContainer { get; } = new UIContainer();
}