using System.Numerics;
using Asteroids.Engine;
using Asteroids.Entities;
using ImGuiNET;

namespace Asteroids.Behaviors;

public class UIBehavior : IBehavior
{
    public void Update(UpdateContext context)
    {
        ImGui.SetNextWindowPos(new Vector2(context.EngineVars.ScreenDimensions.X - 310, 10));
        ImGui.SetNextWindowSize(new Vector2(300, 100));

        if (ImGui.Begin("Players", ImGuiWindowFlags.NoResize))
        {
            if (ImGui.BeginTable("Players", 3))
            {
                ImGui.TableSetupColumn("name", ImGuiTableColumnFlags.None, 0.7f);
                ImGui.TableSetupColumn("status", ImGuiTableColumnFlags.None, 0.15f);
                ImGui.TableSetupColumn("score", ImGuiTableColumnFlags.None, 0.15f);

                foreach (Player player in context.PlayerController.Players)
                {
                    ImGui.TableNextRow();

                    ImGui.TableNextColumn();
                    ImGui.Text($"{player.Name}");
                    ImGui.TableNextColumn();
                    ImGui.Text($"{(player.Alive ? "Alive" : "Dead")}");
                    ImGui.TableNextColumn();
                    ImGui.Text($"{player.Score}");
                }

                ImGui.EndTable();
            }

            ImGui.End();
        }
    }

    public bool Persistent
    {
        get => true;
    }
}