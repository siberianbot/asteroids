using System.Numerics;
using Asteroids.Engine;
using ImGuiNET;
using Silk.NET.Input;

namespace Asteroids.UI;

public class DebugUI : IClientUI
{
    private readonly Engine.Engine _engine;
    private long _subscriptionIdx;
    private bool _visible;

    public DebugUI(Engine.Engine engine)
    {
        _engine = engine;
    }

    public void Initialize()
    {
        _subscriptionIdx = _engine.EventQueue.Subscribe(EventType.KeyPress, @event =>
        {
            if (@event.Key != Key.F12)
            {
                return;
            }

            _visible = !_visible;
        });
    }

    public void Terminate()
    {
        _engine.EventQueue.Unsubscribe(EventType.KeyPress, _subscriptionIdx);
    }

    public void Update()
    {
        if (!_visible)
        {
            return;
        }

        ImGui.SetNextWindowPos(Vector2.Zero);
        ImGui.SetNextWindowSize(new Vector2(400, _engine.Viewport.Size.Y));
        ImGui.SetNextWindowBgAlpha(0.5f);

        if (ImGui.Begin("Debug Menu", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize))
        {
            if (ImGui.CollapsingHeader("Server Info"))
            {
                if (_engine.Server == null)
                {
                    ImGui.Text("Not connected");
                }
                else
                {
                    ImGui.Text($"State: {_engine.Server.State}");
                }
            }

            ImGui.End();
        }
    }
}