using System.Numerics;
using Asteroids.Entities;
using ImGuiNET;

namespace Asteroids.Behaviors;

public class UIBehavior : IBehavior
{
    public void Update(UpdateContext context)
    {
        ImGui.SetNextWindowPos(new Vector2(context.DependencyContainer.ScreenController.ScreenDimensions.X - 310, 10));
        ImGui.SetNextWindowSize(new Vector2(300, 100));

        if (ImGui.Begin("Players", ImGuiWindowFlags.NoResize))
        {
            if (ImGui.BeginTable("Players", 2))
            {
                foreach (Player player in context.DependencyContainer.PlayerController.Players)
                {
                    ImGui.TableNextRow();

                    ImGui.TableNextColumn();
                    ImGui.Text($"{player.Name}");
                    ImGui.TableNextColumn();
                    ImGui.Text($"{(player.Alive ? "Alive" : "Dead")}");
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