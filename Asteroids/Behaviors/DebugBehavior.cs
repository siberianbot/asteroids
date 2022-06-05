using System.Numerics;
using Asteroids.Controllers;
using Asteroids.Engine;
using ImGuiNET;
using Silk.NET.Input;

namespace Asteroids.Behaviors;

public class DebugBehavior : IBehavior
{
    private readonly EventQueue _eventQueue;
    private readonly EngineVars _engineVars;
    private readonly Vars _vars;
    private readonly SceneController _sceneController;
    private long _subscriptionIdx;

    public DebugBehavior(EventQueue eventQueue, EngineVars engineVars, Vars vars, SceneController sceneController)
    {
        _eventQueue = eventQueue;
        _engineVars = engineVars;
        _vars = vars;
        _sceneController = sceneController;
    }

    public void Initialize()
    {
        _subscriptionIdx = _eventQueue.Subscribe(EventType.KeyPress, @event =>
        {
            if (@event.Key != Key.F12)
            {
                return;
            }

            bool value = _vars.GetVar(Constants.Vars.Debug_Enabled, false);

            _vars.SetVar(Constants.Vars.Debug_Enabled, !value);
        });
    }

    public void Terminate()
    {
        _eventQueue.Unsubscribe(EventType.KeyPress, _subscriptionIdx);
    }

    public void Update(UpdateContext context)
    {
        if (!_vars.GetVar(Constants.Vars.Debug_Enabled, false))
        {
            return;
        }

        ImGui.SetNextWindowPos(new Vector2(10, 10));
        ImGui.SetNextWindowSize(new Vector2(400, 200));

        if (ImGui.Begin("Debug Menu", ImGuiWindowFlags.NoResize))
        {
            if (ImGui.CollapsingHeader("Frame Time"))
            {
                ImGui.Text($"Update Time: {Math.Round(_engineVars.UpdateTimeMs, 2)} ms");
                ImGui.Text($"Render Time: {Math.Round(_engineVars.RenderTimeMs, 2)} ms");
                ImGui.Text($"FPS: {Math.Round(1000 / _engineVars.RenderTimeMs)}");
            }

            if (ImGui.CollapsingHeader("Scenes"))
            {
                if (ImGui.Button("Testbed"))
                {
                    _sceneController.ChangeScene(Constants.Scenes.Testbed);
                }

                if (ImGui.Button("Asteroids Demo"))
                {
                    _sceneController.ChangeScene(Constants.Scenes.AsteroidsDemo);
                }

                if (ImGui.Button("Spaceship Demo"))
                {
                    _sceneController.ChangeScene(Constants.Scenes.SpaceshipDemo);
                }

                if (ImGui.Button("Asteroid Collision"))
                {
                    _sceneController.ChangeScene(Constants.Scenes.AsteroidCollision);
                }

                if (ImGui.Button("Playable Demo"))
                {
                    _sceneController.ChangeScene(Constants.Scenes.PlayableDemo);
                }
            }

            if (ImGui.CollapsingHeader("Engine"))
            {
                float timeMultiplier = _vars.GetVar(Constants.Vars.Engine_TimeMultiplier, 1.0f);
                if (ImGui.SliderFloat("Time multiplier", ref timeMultiplier, -5.0f, 5.0f))
                {
                    _vars.SetVar(Constants.Vars.Engine_TimeMultiplier, timeMultiplier);
                }

                if (ImGui.Button("Stop"))
                {
                    _vars.SetVar(Constants.Vars.Engine_TimeMultiplier, 0.0f);
                }

                if (ImGui.Button("TOOO SLOW"))
                {
                    _vars.SetVar(Constants.Vars.Engine_TimeMultiplier, 0.063f);
                }

                if (ImGui.Button("Reset time multiplier"))
                {
                    _vars.SetVar(Constants.Vars.Engine_TimeMultiplier, 1.0f);
                }
            }

            if (ImGui.CollapsingHeader("Physics"))
            {
                if (ImGui.Button("Toggle bounding box rendering"))
                {
                    bool value = _vars.GetVar(Constants.Vars.Physics_ShowBoundingBox, false);

                    _vars.SetVar(Constants.Vars.Physics_ShowBoundingBox, !value);
                }

                if (ImGui.Button("Toggle collider rendering"))
                {
                    bool value = _vars.GetVar(Constants.Vars.Physics_ShowCollider, false);

                    _vars.SetVar(Constants.Vars.Physics_ShowCollider, !value);
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