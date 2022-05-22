using System.Drawing;
using System.Numerics;
using Asteroids.Entities;
using Asteroids.Rendering;
using ImGuiNET;
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

    private InputController _inputController;
    private ImGuiController _imguiController;
    private AsteroidRenderer _asteroidRenderer;
    private Camera _camera;

    private readonly List<IEntity> _entities = new List<IEntity>();
    private readonly AsteroidFactory _asteroidFactory = new AsteroidFactory();

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

        _inputController = new InputController(_input);
        _imguiController = new ImGuiController(_gl, _window, _input);

        Asteroid asteroid = _asteroidFactory.Create(Vector2.Zero, Vector2.Zero);
        _camera = new Camera(asteroid);
        _entities.Add(asteroid);
        _entities.Add(_asteroidFactory.Create(new Vector2(-2.5f, 0f), Vector2.Zero));
        _entities.Add(_asteroidFactory.Create(new Vector2(+2.5f, 0f), Vector2.Zero));

        _asteroidRenderer = new AsteroidRenderer(_gl, _camera);

        OnResize(_window.Size);
    }

    private void OnRender(double delta)
    {
        _gl.ClearColor(Color.FromArgb(255, 0, 0, 0));
        _gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _imguiController.Render();

        RenderContext context = new RenderContext
        {
            AsteroidRenderer = _asteroidRenderer
        };

        foreach (IEntity entity in _entities)
        {
            entity.Render(context);
        }
    }

    private void OnUpdate(double delta)
    {
        _imguiController.Update((float)delta);

        UpdateContext context = new UpdateContext
        {
            Delta = (float)delta
        };

        foreach (IEntity entity in _entities)
        {
            entity.Update(context);
        }
    }

    private void OnResize(Vector2D<int> dimensions)
    {
        _gl.Viewport(dimensions);
        _camera.Dimensions = dimensions;
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

        _asteroidRenderer.Dispose();
        _window.Dispose();

        _disposed = true;
    }

    #endregion
}