using System.Numerics;
using Asteroids.Engine;
using ImGuiNET;
using Silk.NET.Input;

namespace Asteroids.UI;

public class MenuUI : IClientUI
{
    private readonly Engine.Engine _engine;
    private long _keyPressSubscriptionIdx;
    private long _showMenuSubscriptionIdx;
    private long _hideMenuSubscriptionIdx;
    private bool _visible;

    public MenuUI(Engine.Engine engine)
    {
        _engine = engine;
    }

    public void Initialize()
    {
        _keyPressSubscriptionIdx = _engine.EventQueue.Subscribe(EventType.KeyPress, @event =>
        {
            if (@event.Key != Key.Escape)
            {
                return;
            }

            _visible = !_visible;
        });

        _hideMenuSubscriptionIdx = _engine.EventQueue.Subscribe(EventType.HideMenu, @event =>
        {
            if (@event.MenuName != nameof(MenuUI))
            {
                return;
            }

            _visible = true;
        });

        _showMenuSubscriptionIdx = _engine.EventQueue.Subscribe(EventType.ShowMenu, @event =>
        {
            if (@event.MenuName != nameof(MenuUI))
            {
                return;
            }

            _visible = false;
        });

        _visible = true;
    }

    public void Terminate()
    {
        _engine.EventQueue.Unsubscribe(EventType.KeyPress, _keyPressSubscriptionIdx);
        _engine.EventQueue.Unsubscribe(EventType.HideMenu, _hideMenuSubscriptionIdx);
        _engine.EventQueue.Unsubscribe(EventType.ShowMenu, _showMenuSubscriptionIdx);
    }

    public void Update()
    {
        if (!_visible)
        {
            return;
        }

        ImGui.SetNextWindowSize(new Vector2(400, 400));
        ImGui.SetNextWindowPos(new Vector2(_engine.Viewport.Size.X / 2 - 200, _engine.Viewport.Size.Y / 2 - 200));
        ImGui.SetNextWindowBgAlpha(0.5f);

        if (ImGui.Begin("Asteroids", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize))
        {
            if (ImGui.Button("New Game", new Vector2(-1f, 40f)))
            {
                _engine.EventQueue.Push(new Event
                {
                    EventType = EventType.SceneChange,
                    SceneName = Constants.Scenes.PlayableDemo
                });

                _engine.Client = _engine.Server.Join("Asteroids Player");

                _visible = false;
            }

            if (ImGui.Button("Exit", new Vector2(-1f, 40f)))
            {
                _engine.Viewport.Close();
            }

            ImGui.End();
        }
    }
}