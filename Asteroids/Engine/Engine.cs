using System.Numerics;
using Asteroids.Entities;
using Asteroids.Rendering;
using ImGuiNET;
using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace Asteroids.Engine;

public sealed class Engine : IDisposable
{
    private readonly DependencyContainer _dependencyContainer;

    public Engine()
    {
        IWindow window = Window.Create(WindowOptions.Default);
        window.Load += InitWindow;
        window.Render += OnRender;
        window.Update += OnUpdate;
        window.FramebufferResize += OnResize;

        _dependencyContainer = new DependencyContainer(this, window);
    }

    public void Run()
    {
        _dependencyContainer.Window.Run();
    }

    private void InitWindow()
    {
        _dependencyContainer.SceneController.ChangeScene(Constants.Testbed);

        OnResize(_dependencyContainer.Window.Size);
    }

    private void OnRender(double delta)
    {
        _dependencyContainer.Renderer.Clear();

        _dependencyContainer.EntityController.ForEachEntity(entity => _dependencyContainer.Renderer.Render(entity));
        _dependencyContainer.ImGuiController.Render();
    }

    private void OnUpdate(double delta)
    {
        _dependencyContainer.ImGuiController.Update((float)delta);

        ImGui.Begin("Scenes");

        if (ImGui.Button("Testbed"))
        {
            _dependencyContainer.SceneController.ChangeScene(Constants.Testbed);
        }

        if (ImGui.Button("Asteroids Demo"))
        {
            _dependencyContainer.SceneController.ChangeScene(Constants.AsteroidsDemo);
        }

        if (ImGui.Button("Spaceship Demo"))
        {
            _dependencyContainer.SceneController.ChangeScene(Constants.SpaceshipDemo);
        }

        ImGui.End();

        UpdateContext context = new UpdateContext
        {
            Delta = (float)delta,
            InputController = _dependencyContainer.InputController // TODO: use dependency container
        };

        _dependencyContainer.EntityController.ForEachEntity(entity => entity.Update(context));
    }

    private void OnResize(Vector2D<int> dimensions)
    {
        _dependencyContainer.Renderer.UpdateDimensions(dimensions);
        _dependencyContainer.CameraController.UpdateDimensions(dimensions);
    }

    #region IDisposable

    private bool _disposed;

    ~Engine()
    {
        InternalDispose();
    }

    public void Dispose()
    {
        InternalDispose();
        GC.SuppressFinalize(this);
    }

    private void InternalDispose()
    {
        if (_disposed)
        {
            return;
        }

        _dependencyContainer.Dispose();

        _disposed = true;
    }

    #endregion
}