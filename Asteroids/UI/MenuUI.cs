using System.Numerics;
using ImGuiNET;

namespace Asteroids.UI;

public class MenuUI : IClientUI
{
    private readonly Engine.Engine _engine;

    public MenuUI(Engine.Engine engine)
    {
        _engine = engine;
    }

    public void Update()
    {
        ImGui.SetNextWindowSize(new Vector2(400, 400));
        ImGui.SetNextWindowPos(new Vector2(_engine.Viewport.Size.X / 2 - 200, _engine.Viewport.Size.Y / 2 - 200));

        if (ImGui.Begin("Asteroids", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize))
        {
            if (ImGui.Button("New Game", new Vector2(-1f, 40f)))
            {
                _engine.StartServer();
            }

            if (ImGui.Button("Exit", new Vector2(-1f, 40f)))
            {
                _engine.Viewport.Close();
            }

            ImGui.End();
        }
    }
}