using System.Numerics;
using ImGuiNET;

namespace Asteroids.UI;

public class PlayerStatsUI : ClientUI
{
    private readonly Engine.Engine _engine;

    public PlayerStatsUI(Engine.Engine engine) : base(engine.Vars, Constants.Vars.ClientUIShowPlayerStats)
    {
        _engine = engine;
    }

    protected override void OnUpdate()
    {
        if (_engine.Client?.Player == null)
        {
            return;
        }

        ImGui.SetNextWindowSize(new Vector2(150, 50));
        ImGui.SetNextWindowPos(new Vector2(0, _engine.Viewport.Size.Y - 50));
        ImGui.SetNextWindowBgAlpha(0.5f);

        if (ImGui.Begin("Player Statistics", ImGuiWindowFlags.NoResize |
                                             ImGuiWindowFlags.NoCollapse |
                                             ImGuiWindowFlags.NoInputs |
                                             ImGuiWindowFlags.NoTitleBar))
        {
            ImGui.Text($"Score: {_engine.Client.Player.Score}");
            ImGui.Text($"{_engine.Client.Name}");

            ImGui.End();
        }
    }
}