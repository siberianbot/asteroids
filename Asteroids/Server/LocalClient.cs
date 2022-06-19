using Asteroids.Engine;
using Asteroids.Entities;

namespace Asteroids.Server;

public class LocalClient : IClient
{
    private readonly Vars _vars;
    private IServer? _server;

    public LocalClient(Vars vars)
    {
        _vars = vars;
    }

    public Player? Player { get; set; }

    public Camera? Camera { get; set; }

    public string Name
    {
        get => _vars.GetVar(Constants.Vars.ClientName, "Asteroids Player");
    }

    public bool IsJoined
    {
        get => _server != null;
    }
}