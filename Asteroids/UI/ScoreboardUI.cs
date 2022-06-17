using System.Numerics;
using Asteroids.Entities;
using ImGuiNET;

namespace Asteroids.UI;

public class ScoreboardUI : ClientUI
{
    private readonly Engine.Engine _engine;

    public ScoreboardUI(Engine.Engine engine) : base(engine.Vars, Constants.Vars.ClientUIShowScoreboard)
    {
        _engine = engine;
    }

    protected override void OnUpdate()
    {
        if (_engine.Server?.PlayerCollection == null)
        {
            return;
        }

        ImGui.SetNextWindowSize(new Vector2(500, 300));
        ImGui.SetNextWindowPos(new Vector2(_engine.Viewport.Size.X / 2 - 250, _engine.Viewport.Size.Y / 2 - 150));
        ImGui.SetNextWindowBgAlpha(0.5f);

        if (ImGui.Begin("Scoreboard", ImGuiWindowFlags.NoResize |
                                      ImGuiWindowFlags.NoCollapse |
                                      ImGuiWindowFlags.NoInputs |
                                      ImGuiWindowFlags.NoTitleBar))
        {
            if (ImGui.BeginTable("scoreboard", 3))
            {
                ImGui.TableSetupColumn("name", ImGuiTableColumnFlags.None, 0.75f);
                ImGui.TableSetupColumn("status", ImGuiTableColumnFlags.None, 0.10f);
                ImGui.TableSetupColumn("score", ImGuiTableColumnFlags.None, 0.15f);

                foreach (Player player in _engine.Server.PlayerCollection.Players)
                {
                    ImGui.TableNextRow();

                    ImGui.TableNextColumn();
                    ImGui.Text(player.Name);
                    ImGui.TableNextColumn();
                    ImGui.Text(player.Alive ? "Alive" : "Dead");
                    ImGui.TableNextColumn();
                    ImGui.Text($"{player.Score}");
                }

                ImGui.EndTable();
            }

            ImGui.End();
        }
    }
}