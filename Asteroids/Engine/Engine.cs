using System.Numerics;
using Asteroids.Entities;
using Asteroids.Rendering;
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
        Spawner spawner = _dependencyContainer.Spawner;

        Spaceship spaceship = spawner.SpawnSpaceship(new Vector2(+2.5f, -2.0f));
        _dependencyContainer.CameraController.CurrentCamera = new Camera(spaceship);

        spawner.SpawnAsteroid(Vector2.Zero, Vector2.Zero);
        spawner.SpawnAsteroid(new Vector2(-2.5f, 0f), Vector2.Zero);
        spawner.SpawnAsteroid(new Vector2(+2.5f, 0f), Vector2.Zero);
        spawner.SpawnAsteroid(new Vector2(-5.0f, 0f), new Vector2(1.0f, 0.0f));
        spawner.SpawnBullet(spaceship, new Vector2(-2.5f, -2.0f), new Vector2(0.0f, 0.0f));

        OnResize(_dependencyContainer.Window.Size);
    }

    private void OnRender(double delta)
    {
        _dependencyContainer.Renderer.Clear();

        _dependencyContainer.ImGuiController.Render();
        _dependencyContainer.EntityController.ForEachEntity(entity => _dependencyContainer.Renderer.Render(entity));
    }

    private void OnUpdate(double delta)
    {
        _dependencyContainer.ImGuiController.Update((float)delta);

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