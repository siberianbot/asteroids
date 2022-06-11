using Asteroids.Server;
using Asteroids.UI;

namespace Asteroids.Engine;

public sealed class Engine : IDisposable
{
    private bool _disposed;

    private readonly EventQueue _eventQueue = new EventQueue();
    private readonly LocalClient _client;
    private readonly Viewport _viewport;
    private IServer? _server;

    public Engine()
    {
        _client = new LocalClient(this);
        _viewport = new Viewport(this);
    }

    public void Run()
    {
        _client.ClientUIContainer.Set(3, new DebugUI(this));
        _client.ClientUIContainer.Set(2, new MenuUI(this));

        _eventQueue.Push(new Event { EventType = EventType.EngineReady });

        _viewport.Run();

        _client.ClientUIContainer.Clear();

        StopServer();
    }

    public void StartServer()
    {
        StopServer();

        _server = new LocalServer(_eventQueue);
        _server.Start();
    }

    public void StopServer()
    {
        _server?.Stop();
    }

    public IClient Client
    {
        get => _client;
    }

    public IServer? Server
    {
        get => _server;
    }

    public EventQueue EventQueue
    {
        get => _eventQueue;
    }

    public Viewport Viewport
    {
        get => _viewport;
    }

    #region IDisposable

    ~Engine()
    {
        InternalDispose();
    }

    public void Dispose()
    {
        InternalDispose();
        GC.SuppressFinalize(this);
    }

    private void InternalDispose()
    {
        if (_disposed)
        {
            return;
        }

        _viewport.Dispose();

        _disposed = true;
    }

    #endregion
}