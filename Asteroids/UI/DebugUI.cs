using System.Numerics;
using ImGuiNET;

namespace Asteroids.UI;

public class DebugUI : ClientUI
{
    private readonly Engine.Engine _engine;

    public DebugUI(Engine.Engine engine) : base(engine.Vars, Constants.Vars.ClientUIShowDebug)
    {
        _engine = engine;
    }

    protected override void OnUpdate()
    {
        ImGui.SetNextWindowPos(Vector2.Zero);
        ImGui.SetNextWindowSize(new Vector2(400, _engine.Viewport.Size.Y));
        ImGui.SetNextWindowBgAlpha(0.5f);

        if (ImGui.Begin("Debug Menu", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize))
        {
            if (ImGui.CollapsingHeader("Server Info"))
            {
                if (_engine.ClientServerHost.Server == null)
                {
                    ImGui.Text("Not connected");
                }
                else
                {
                    ImGui.Text($"State: {_engine.ClientServerHost.Server.State}");
                }
            }

            ImGui.End();
        }
    }
}