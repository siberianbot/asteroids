using System.Numerics;
using ImGuiNET;
using Silk.NET.Windowing;

namespace Asteroids.Client.UI;

public class MenuUI : IClientUI
{
    private readonly IWindow _window;

    public MenuUI(IWindow window)
    {
        _window = window;
    }

    public void Update()
    {
        ImGui.SetNextWindowSize(new Vector2(400, 400));
        ImGui.SetNextWindowPos(new Vector2(_window.Size.X / 2 - 200, _window.Size.Y / 2 - 200));

        if (ImGui.Begin("Asteroids", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize))
        {
            if (ImGui.Button("New Game", new Vector2(-1f, 40f)))
            {
            }

            ImGui.End();
        }
    }
}