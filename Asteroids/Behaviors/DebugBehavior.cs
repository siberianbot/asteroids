using System.Numerics;
using Asteroids.Controllers;
using Asteroids.Engine;
using ImGuiNET;
using Silk.NET.Input;

namespace Asteroids.Behaviors;

public class DebugBehavior : IBehavior
{
    public void Update(UpdateContext context)
    {
        if (context.Controllers.GetController<InputController>().IsKeyPressed(Key.F12))
        {
            bool value = context.GlobalVars.GetVar(Constants.Vars.Debug_Enabled, false);

            context.GlobalVars.SetVar(Constants.Vars.Debug_Enabled, !value);
        }

        if (!context.GlobalVars.GetVar(Constants.Vars.Debug_Enabled, false))
        {
            return;
        }

        ImGui.SetNextWindowPos(new Vector2(10, 10));
        ImGui.SetNextWindowSize(new Vector2(400, 200));

        if (ImGui.Begin("Debug Menu", ImGuiWindowFlags.NoResize))
        {
            if (ImGui.CollapsingHeader("Frame Time"))
            {
                ImGui.Text($"Update Time: {Math.Round(context.EngineVars.UpdateTimeMs, 2)} ms");
                ImGui.Text($"Render Time: {Math.Round(context.EngineVars.RenderTimeMs, 2)} ms");
                ImGui.Text($"FPS: {Math.Round(1000 / context.EngineVars.RenderTimeMs)}");
            }

            if (ImGui.CollapsingHeader("Scenes"))
            {
                if (ImGui.Button("Testbed"))
                {
                    context.Controllers.GetController<SceneController>().ChangeScene(Constants.Scenes.Testbed);
                }

                if (ImGui.Button("Asteroids Demo"))
                {
                    context.Controllers.GetController<SceneController>().ChangeScene(Constants.Scenes.AsteroidsDemo);
                }

                if (ImGui.Button("Spaceship Demo"))
                {
                    context.Controllers.GetController<SceneController>().ChangeScene(Constants.Scenes.SpaceshipDemo);
                }

                if (ImGui.Button("Asteroid Collision"))
                {
                    context.Controllers.GetController<SceneController>().ChangeScene(Constants.Scenes.AsteroidCollision);
                }

                if (ImGui.Button("Playable Demo"))
                {
                    context.Controllers.GetController<SceneController>().ChangeScene(Constants.Scenes.PlayableDemo);
                }
            }

            if (ImGui.CollapsingHeader("Engine"))
            {
                float timeMultiplier = context.GlobalVars.GetVar(Constants.Vars.Engine_TimeMultiplier, 1.0f);
                if (ImGui.SliderFloat("Time multiplier", ref timeMultiplier, -5.0f, 5.0f))
                {
                    context.GlobalVars.SetVar(Constants.Vars.Engine_TimeMultiplier, timeMultiplier);
                }

                if (ImGui.Button("Stop"))
                {
                    context.GlobalVars.SetVar(Constants.Vars.Engine_TimeMultiplier, 0.0f);
                }

                if (ImGui.Button("TOOO SLOW"))
                {
                    context.GlobalVars.SetVar(Constants.Vars.Engine_TimeMultiplier, 0.063f);
                }

                if (ImGui.Button("Reset time multiplier"))
                {
                    context.GlobalVars.SetVar(Constants.Vars.Engine_TimeMultiplier, 1.0f);
                }
            }

            if (ImGui.CollapsingHeader("Physics"))
            {
                if (ImGui.Button("Toggle bounding box rendering"))
                {
                    bool value = context.GlobalVars.GetVar(Constants.Vars.Physics_ShowBoundingBox, false);

                    context.GlobalVars.SetVar(Constants.Vars.Physics_ShowBoundingBox, !value);
                }

                if (ImGui.Button("Toggle collider rendering"))
                {
                    bool value = context.GlobalVars.GetVar(Constants.Vars.Physics_ShowCollider, false);

                    context.GlobalVars.SetVar(Constants.Vars.Physics_ShowCollider, !value);
                }
            }

            ImGui.End();
        }
    }

    public bool Persistent
    {
        get => true;
    }
}