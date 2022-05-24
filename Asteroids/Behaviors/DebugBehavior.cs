using Asteroids.Entities;
using ImGuiNET;

namespace Asteroids.Behaviors;

public class DebugBehavior : IBehavior
{
    public void Update(UpdateContext context)
    {
        context.DependencyContainer.ImGuiController.Update(context.Delta);

        ImGui.Begin("Debug Menu");

        if (ImGui.Button("Testbed"))
        {
            context.DependencyContainer.SceneController.ChangeScene(Constants.Scenes.Testbed);
        }

        if (ImGui.Button("Asteroids Demo"))
        {
            context.DependencyContainer.SceneController.ChangeScene(Constants.Scenes.AsteroidsDemo);
        }

        if (ImGui.Button("Spaceship Demo"))
        {
            context.DependencyContainer.SceneController.ChangeScene(Constants.Scenes.SpaceshipDemo);
        }

        ImGui.End();
    }

    public bool Persistent
    {
        get => true;
    }
}