using Asteroids.Entities;
using ImGuiNET;

namespace Asteroids.Behaviors;

public class DebugBehavior : IBehavior
{
    public void Update(UpdateContext context)
    {
        context.DependencyContainer.ImGuiController.Update(context.Delta);

        ImGui.Begin("Debug Menu");

        if (ImGui.CollapsingHeader("Frame Time"))
        {
            ImGui.Text($"Update Time: {Math.Round(context.DependencyContainer.Engine.UpdateTimeMs, 2)} ms");
            ImGui.Text($"Render Time: {Math.Round(context.DependencyContainer.Engine.RenderTimeMs, 2)} ms");
            ImGui.Text($"FPS: {Math.Round(1000 / context.DependencyContainer.Engine.RenderTimeMs)}");
        }

        if (ImGui.CollapsingHeader("Scenes"))
        {
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

            if (ImGui.Button("Asteroid Collision"))
            {
                context.DependencyContainer.SceneController.ChangeScene(Constants.Scenes.AsteroidCollision);
            }
        }

        if (ImGui.CollapsingHeader("Engine"))
        {
            float timeMultiplier = context.DependencyContainer.GlobalVars.GetVar(Constants.Vars.Engine_TimeMultiplier, 1.0f);
            if (ImGui.SliderFloat("Time multiplier", ref timeMultiplier, -5.0f, 5.0f))
            {
                context.DependencyContainer.GlobalVars.SetVar(Constants.Vars.Engine_TimeMultiplier, timeMultiplier);
            }

            if (ImGui.Button("Stop"))
            {
                context.DependencyContainer.GlobalVars.SetVar(Constants.Vars.Engine_TimeMultiplier, 0.0f);
            }
            
            if (ImGui.Button("TOOO SLOW"))
            {
                context.DependencyContainer.GlobalVars.SetVar(Constants.Vars.Engine_TimeMultiplier, 0.063f);
            }

            if (ImGui.Button("Reset time multiplier"))
            {
                context.DependencyContainer.GlobalVars.SetVar(Constants.Vars.Engine_TimeMultiplier, 1.0f);
            }
        }

        if (ImGui.CollapsingHeader("Physics"))
        {
            if (ImGui.Button("Toggle bounding box rendering"))
            {
                bool value = context.DependencyContainer.GlobalVars.GetVar(Constants.Vars.Physics_ShowBoundingBox, false);

                context.DependencyContainer.GlobalVars.SetVar(Constants.Vars.Physics_ShowBoundingBox, !value);
            }

            if (ImGui.Button("Toggle collider rendering"))
            {
                bool value = context.DependencyContainer.GlobalVars.GetVar(Constants.Vars.Physics_ShowCollider, false);

                context.DependencyContainer.GlobalVars.SetVar(Constants.Vars.Physics_ShowCollider, !value);
            }
        }

        ImGui.End();
    }

    public bool Persistent
    {
        get => true;
    }
}