using System.Numerics;
using Asteroids.Engine;
using ImGuiNET;

namespace Asteroids.UI;

public class MenuUI : ClientUI
{
    private readonly Engine.Engine _engine;

    public MenuUI(Engine.Engine engine) : base(engine.Vars, Constants.Vars.ClientUIShowMenu)
    {
        _engine = engine;
    }

    public override void Initialize()
    {
        SetVisibility(true);
    }

    protected override void OnUpdate()
    {
        ImGui.SetNextWindowSize(new Vector2(400, 400));
        ImGui.SetNextWindowPos(new Vector2(_engine.Viewport.Size.X / 2 - 200, _engine.Viewport.Size.Y / 2 - 200));
        ImGui.SetNextWindowBgAlpha(0.5f);

        if (ImGui.Begin("Asteroids", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize))
        {
            if (ImGui.Button("New Game", new Vector2(-1f, 40f)))
            {
                _engine.EventQueue.Push(new Event
                {
                    EventType = EventType.SceneChange,
                    SceneName = Constants.Scenes.PlayableDemo
                });

                _engine.Client = _engine.Server!.Join("Asteroids Player");
                _engine.InputProcessor.Enabled = true;

                SetVisibility(false);
            }

            if (ImGui.Button("Exit", new Vector2(-1f, 40f)))
            {
                _engine.Viewport.Close();
            }

            ImGui.End();
        }
    }
}