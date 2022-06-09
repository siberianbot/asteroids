using System.Numerics;
using Asteroids.Client.Controllers;
using Asteroids.Engine;
using ImGuiNET;
using Silk.NET.Input;

namespace Asteroids.Client.UI;

public interface IClientUI
{
    void Initialize()
    {
        //
    }

    void Terminate()
    {
        //
    }

    void Update();
}

public class DebugUI : IClientUI
{
    private readonly EventQueue _eventQueue;
    private readonly Vars _vars;
    private long _subscriptionIdx;

    public DebugUI(EventQueue eventQueue, Vars vars)
    {
        _eventQueue = eventQueue;
        _vars = vars;
    }

    public void Initialize()
    {
        _subscriptionIdx = _eventQueue.Subscribe(EventType.KeyPress, @event =>
        {
            if (@event.Key != Key.F12)
            {
                return;
            }

            bool value = _vars.GetVar(Constants.Vars.DebugEnabled, false);

            _vars.SetVar(Constants.Vars.DebugEnabled, !value);
        });
    }

    public void Terminate()
    {
        _eventQueue.Unsubscribe(EventType.KeyPress, _subscriptionIdx);
    }

    public void Update()
    {
        if (!_vars.GetVar(Constants.Vars.DebugEnabled, false))
        {
            return;
        }

        ImGui.SetNextWindowPos(new Vector2(10, 10));
        ImGui.SetNextWindowSize(new Vector2(400, 200));

        if (ImGui.Begin("Debug Menu", ImGuiWindowFlags.NoResize))
        {
            if (ImGui.CollapsingHeader("Frame Time"))
            {
                // ImGui.Text($"Update Time: {Math.Round(_engineVars.UpdateTimeMs, 2)} ms");
                // ImGui.Text($"Render Time: {Math.Round(_engineVars.RenderTimeMs, 2)} ms");
                // ImGui.Text($"FPS: {Math.Round(1000 / _engineVars.RenderTimeMs)}");
            }

            if (ImGui.CollapsingHeader("Scenes"))
            {
                // TODO:
                ImGui.Text("Scenes are not available yet");
                
                // if (ImGui.Button("Testbed"))
                // {
                //     _sceneController.ChangeScene(Constants.Scenes.Testbed);
                // }
                //
                // if (ImGui.Button("Asteroids Demo"))
                // {
                //     _sceneController.ChangeScene(Constants.Scenes.AsteroidsDemo);
                // }
                //
                // if (ImGui.Button("Spaceship Demo"))
                // {
                //     _sceneController.ChangeScene(Constants.Scenes.SpaceshipDemo);
                // }
                //
                // if (ImGui.Button("Asteroid Collision"))
                // {
                //     _sceneController.ChangeScene(Constants.Scenes.AsteroidCollision);
                // }
                //
                // if (ImGui.Button("Playable Demo"))
                // {
                //     _sceneController.ChangeScene(Constants.Scenes.PlayableDemo);
                // }
            }

            if (ImGui.CollapsingHeader("Engine"))
            {
                float timeMultiplier = _vars.GetVar(Constants.Vars.EngineTimeMultiplier, 1.0f);
                if (ImGui.SliderFloat("Time multiplier", ref timeMultiplier, -5.0f, 5.0f))
                {
                    _vars.SetVar(Constants.Vars.EngineTimeMultiplier, timeMultiplier);
                }

                if (ImGui.Button("Stop"))
                {
                    _vars.SetVar(Constants.Vars.EngineTimeMultiplier, 0.0f);
                }

                if (ImGui.Button("TOOO SLOW"))
                {
                    _vars.SetVar(Constants.Vars.EngineTimeMultiplier, 0.063f);
                }

                if (ImGui.Button("Reset time multiplier"))
                {
                    _vars.SetVar(Constants.Vars.EngineTimeMultiplier, 1.0f);
                }
            }

            if (ImGui.CollapsingHeader("Physics"))
            { if (ImGui.Button("Toggle activation radius rendering"))
                {
                    bool value = _vars.GetVar(Constants.Vars.PhysicsShowActivationRadius, false);

                    _vars.SetVar(Constants.Vars.PhysicsShowActivationRadius, !value);
                }

                if (ImGui.Button("Toggle collider rendering"))
                {
                    bool value = _vars.GetVar(Constants.Vars.PhysicsShowCollider, false);

                    _vars.SetVar(Constants.Vars.PhysicsShowCollider, !value);
                }
            }

            ImGui.End();
        }
    }
}