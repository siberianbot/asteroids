using Asteroids.Entities;
using ImGuiNET;

namespace Asteroids.Behaviors;

public class DebugBehavior : IBehavior
{
    public void Update(UpdateContext context)
    {
        context.DependencyContainer.ImGuiController.Update(context.Delta);

        ImGui.Begin("Scenes");

        if (ImGui.Button("Testbed"))
        {
            context.DependencyContainer.SceneController.ChangeScene(Constants.Testbed);
        }

        if (ImGui.Button("Asteroids Demo"))
        {
            context.DependencyContainer.SceneController.ChangeScene(Constants.AsteroidsDemo);
        }

        if (ImGui.Button("Spaceship Demo"))
        {
            context.DependencyContainer.SceneController.ChangeScene(Constants.SpaceshipDemo);
        }

        ImGui.End();
    }

    public bool Persistent
    {
        get => true;
    }
}