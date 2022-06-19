using Asteroids.Input;
using Asteroids.Server;
using Asteroids.UI;
using Silk.NET.Input;

namespace Asteroids.Engine;

public sealed class Engine : IDisposable
{
    private bool _disposed;

    public Engine()
    {
        Viewport = new Viewport(this);
        Client = new LocalClient(Vars);
        Server = new LocalServer(Vars);
    }

    public void Run()
    {
        Server.Start();

        UIContainer.Set(3, new DebugUI(this));
        UIContainer.Set(2, new MenuUI(this));
        UIContainer.Set(1, new ScoreboardUI(this));
        UIContainer.Set(0, new PlayerStatsUI(this));

        InputProcessor.OnKeyPress(Key.Escape,
            () => Vars.SetVar(Constants.Vars.ClientUIShowMenu, !Vars.GetVar(Constants.Vars.ClientUIShowMenu, false)));
        InputProcessor.OnKeyPress(Key.F12,
            () => Vars.SetVar(Constants.Vars.ClientUIShowDebug, !Vars.GetVar(Constants.Vars.ClientUIShowDebug, false)));
        InputProcessor.OnKeyPress(Key.Tab, () => Vars.SetVar(Constants.Vars.ClientUIShowScoreboard, true));
        InputProcessor.OnKeyRelease(Key.Tab, () => Vars.SetVar(Constants.Vars.ClientUIShowScoreboard, false));

        InputProcessor.OnKeyPress(Key.Up, () => Client.PushEvent(new Event
        {
            EventType = EventType.ClientAction,
            Client = Client,
            ClientAction = ClientAction.Accelerate,
            ClientActionState = ClientActionState.Enable
        }));
        InputProcessor.OnKeyRelease(Key.Up, () => Client.PushEvent(new Event
        {
            EventType = EventType.ClientAction,
            Client = Client,
            ClientAction = ClientAction.Accelerate,
            ClientActionState = ClientActionState.Disable
        }));
        InputProcessor.OnKeyPress(Key.Down, () => Client.PushEvent(new Event
        {
            EventType = EventType.ClientAction,
            Client = Client,
            ClientAction = ClientAction.Decelerate,
            ClientActionState = ClientActionState.Enable
        }));
        InputProcessor.OnKeyRelease(Key.Down, () => Client.PushEvent(new Event
        {
            EventType = EventType.ClientAction,
            Client = Client,
            ClientAction = ClientAction.Decelerate,
            ClientActionState = ClientActionState.Disable
        }));
        InputProcessor.OnKeyPress(Key.Left, () => Client.PushEvent(new Event
        {
            EventType = EventType.ClientAction,
            Client = Client,
            ClientAction = ClientAction.TurnLeft,
            ClientActionState = ClientActionState.Enable
        }));
        InputProcessor.OnKeyRelease(Key.Left, () => Client.PushEvent(new Event
        {
            EventType = EventType.ClientAction,
            Client = Client,
            ClientAction = ClientAction.TurnLeft,
            ClientActionState = ClientActionState.Disable
        }));
        InputProcessor.OnKeyPress(Key.Right, () => Client.PushEvent(new Event
        {
            EventType = EventType.ClientAction,
            Client = Client,
            ClientAction = ClientAction.TurnRight,
            ClientActionState = ClientActionState.Enable
        }));
        InputProcessor.OnKeyRelease(Key.Right, () => Client.PushEvent(new Event
        {
            EventType = EventType.ClientAction,
            Client = Client,
            ClientAction = ClientAction.TurnRight,
            ClientActionState = ClientActionState.Disable
        }));
        InputProcessor.OnKeyPress(Key.Z, () => Client.PushEvent(new Event
        {
            EventType = EventType.ClientAction,
            Client = Client,
            ClientAction = ClientAction.Stop,
            ClientActionState = ClientActionState.Enable
        }));
        InputProcessor.OnKeyRelease(Key.Z, () => Client.PushEvent(new Event
        {
            EventType = EventType.ClientAction,
            Client = Client,
            ClientAction = ClientAction.Stop,
            ClientActionState = ClientActionState.Disable
        }));
        InputProcessor.OnKeyPress(Key.Space, () => Client.PushEvent(new Event
        {
            EventType = EventType.ClientAction,
            Client = Client,
            ClientAction = ClientAction.Fire,
            ClientActionState = ClientActionState.Enable
        }));

        Viewport.Run();

        UIContainer.Clear();

        Server.Stop();
    }

    public Vars Vars { get; } = new Vars();

    public UIContainer UIContainer { get; } = new UIContainer();

    public InputProcessor InputProcessor { get; } = new InputProcessor();

    public IClient Client { get; }

    public IServer Server { get; }

    public Viewport Viewport { get; }

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

        Viewport.Dispose();

        _disposed = true;
    }

    #endregion
}