using System.Drawing;
using System.Numerics;
using Asteroids.Entities;
using Asteroids.Rendering;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;

namespace Asteroids.Engine;

public sealed class Engine : IDisposable
{
    private readonly IWindow _window;

    private GL _gl;
    private IInputContext _input;

    private CameraController _cameraController;
    private EntityController _entityController;
    private InputController _inputController;
    private ImGuiController _imguiController;
    private Spawner _spawner;
    private Renderer _renderer;

    public Engine()
    {
        _window = Window.Create(WindowOptions.Default);
        _window.Load += InitWindow;
        _window.Render += OnRender;
        _window.Update += OnUpdate;
        _window.FramebufferResize += OnResize;
    }

    public void Run()
    {
        _window.Run();
    }

    private void InitWindow()
    {
        _input = _window.CreateInput();
        _gl = _window.CreateOpenGL();

        _cameraController = new CameraController();
        _inputController = new InputController(_input);
        _imguiController = new ImGuiController(_gl, _window, _input);
        _entityController = new EntityController();
        _spawner = new Spawner(_entityController);
        _renderer = new Renderer(_gl, _cameraController);

        Spaceship spaceship = _spawner.SpawnSpaceship(new Vector2(+2.5f, -2.0f));
        _cameraController.CurrentCamera = new Camera(spaceship);

        _spawner.SpawnAsteroid(Vector2.Zero, Vector2.Zero);
        _spawner.SpawnAsteroid(new Vector2(-2.5f, 0f), Vector2.Zero);
        _spawner.SpawnAsteroid(new Vector2(+2.5f, 0f), Vector2.Zero);
        _spawner.SpawnAsteroid(new Vector2(-5.0f, 0f), new Vector2(1.0f, 0.0f));
        _spawner.SpawnBullet(spaceship, new Vector2(-2.5f, -2.0f), new Vector2(0.0f, 0.0f));

        OnResize(_window.Size);
    }

    private void OnRender(double delta)
    {
        _gl.ClearColor(Color.FromArgb(255, 0, 0, 0));
        _gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _imguiController.Render();
        _entityController.ForEachEntity(entity => _renderer.Render(entity));
    }

    private void OnUpdate(double delta)
    {
        _imguiController.Update((float)delta);

        UpdateContext context = new UpdateContext
        {
            Delta = (float)delta,
            InputController = _inputController
        };

        _entityController.ForEachEntity(entity => entity.Update(context));
    }

    private void OnResize(Vector2D<int> dimensions)
    {
        _gl.Viewport(dimensions);
        _cameraController.UpdateDimensions(dimensions);
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

        _renderer.Dispose();
        _window.Dispose();

        _disposed = true;
    }

    #endregion
}