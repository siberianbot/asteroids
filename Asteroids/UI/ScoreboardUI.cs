using System.Numerics;
using Asteroids.Server;
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
        if (_engine.Server == null)
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

                foreach (IClient client in _engine.Server.Clients)
                {
                    if (client.Player == null)
                    {
                        continue;
                    }

                    ImGui.TableNextRow();

                    ImGui.TableNextColumn();
                    ImGui.Text(client.Name);
                    ImGui.TableNextColumn();
                    ImGui.Text(client.Player.Alive ? "Alive" : "Dead");
                    ImGui.TableNextColumn();
                    ImGui.Text($"{client.Player.Score}");
                }

                ImGui.EndTable();
            }

            ImGui.End();
        }
    }
}