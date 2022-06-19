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

    public void JoinServer(IServer server)
    {
        if (_server != null)
        {
            LeaveServer();
        }

        _server = server;

        PushEvent(new Event
        {
            EventType = EventType.ClientConnected,
            Client = this,
            ClientName = Name
        });
    }

    public void LeaveServer()
    {
        if (_server == null)
        {
            return;
        }

        PushEvent(new Event
        {
            EventType = EventType.ClientDisconnected,
            Client = this
        });

        _server = null;
    }

    public void PushEvent(Event @event)
    {
        if (_server == null)
        {
            // TODO: I don't know what to do there
            throw new NotImplementedException();
        }

        _server.Push(@event);
    }
}