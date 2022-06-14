using Asteroids.Server;
using Asteroids.UI;

namespace Asteroids.Engine;

public sealed class Engine : IDisposable
{
    private bool _disposed;

    private readonly EventQueue _eventQueue = new EventQueue();
    private readonly Viewport _viewport;

    public Engine()
    {
        _viewport = new Viewport(this);
    }

    public void Run()
    {
        Server = new LocalServer(_eventQueue);
        Server.Start();

        UIContainer.Set(3, new DebugUI(this));
        UIContainer.Set(2, new MenuUI(this));

        _eventQueue.Push(new Event { EventType = EventType.EngineReady });

        _viewport.Run();

        UIContainer.Clear();

        Server.Stop();
    }

    public IClient? Client { get; set; }

    public IServer Server { get; private set; }

    public Vars Vars { get; } = new Vars();

    public UIContainer UIContainer { get; } = new UIContainer();

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