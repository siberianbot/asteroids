using Asteroids.Engine;

namespace Asteroids.Server;

public class ClientServerHost
{
    private readonly Vars _vars;

    public ClientServerHost(Vars vars)
    {
        _vars = vars;
        Client = new LocalClient(vars);
    }

    public void CreateLocalServer()
    {
        if (Server != null)
        {
            LeaveServer();
        }

        Server = new LocalServer(_vars);
        Server.Start();

        while (Server.State != ServerState.Alive)
        {
            // wait for start
        }

        PushEvent(new Event
        {
            EventType = EventType.ClientConnected,
            Client = Client,
            ClientName = Client.Name
        });
    }

    public void LeaveServer()
    {
        if (Server == null)
        {
            return;
        }

        PushEvent(new Event
        {
            EventType = EventType.ClientDisconnected,
            Client = Client
        });

        Server.Stop();
        Server = null;
    }

    public void PushEvent(Event @event)
    {
        if (Server == null)
        {
            // TODO: I don't know what to do there
            throw new NotImplementedException();
        }

        Server.Push(@event);
    }

    public IClient Client { get; }

    public IServer? Server { get; private set; }
}