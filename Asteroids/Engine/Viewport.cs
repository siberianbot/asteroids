using System.Numerics;
using Asteroids.Rendering;
using Asteroids.Utils;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;

namespace Asteroids.Engine;

public class Viewport : IDisposable
{
    private readonly Engine _engine;
    private readonly IWindow _window;
    private readonly Lazy<IInputContext> _inputContext;
    private readonly Lazy<Renderer> _renderer;
    private readonly Lazy<ImGuiController> _imguiController;
    private bool _disposed;

    public Viewport(Engine engine)
    {
        _engine = engine;

        _window = Window.Create(WindowOptions.Default);
        _window.Load += OnLoadWindow;
        _window.Closing += OnClosingWindow;
        _window.Render += OnRender;
        _window.Update += OnUpdate;
        _window.FramebufferResize += OnResize;

        _inputContext = new Lazy<IInputContext>(_window.CreateInput);
        Lazy<GL> gl = new Lazy<GL>(_window.CreateOpenGL);
        _renderer = new Lazy<Renderer>(() => new Renderer(gl.Value));
        _imguiController = new Lazy<ImGuiController>(() => new ImGuiController(gl.Value, _window, _inputContext.Value));
    }

    public void Run()
    {
        _window.Run();
    }

    public void Close()
    {
        _window.Close();
    }

    public Vector2D<int> Size
    {
        get => _window.Size;
    }

    private void OnLoadWindow()
    {
        _renderer.Value.UpdateDimensions(_window.Size);

        foreach (IKeyboard keyboard in _inputContext.Value.Keyboards)
        {
            keyboard.KeyDown += OnKeyPress;
            keyboard.KeyUp += OnKeyRelease;
        }
    }

    private void OnClosingWindow()
    {
        // TODO: it is incorrect to dispose renderer on window close
        _renderer.Value.Dispose();
    }

    private void OnUpdate(double delta)
    {
        _imguiController.Value.Update((float)delta);

        _engine.EventQueue.ExecutePending();
        _engine.Client.ClientUIContainer.Update();
    }

    private void OnRender(double delta)
    {
        _renderer.Value.Clear();

        // IEnumerable<ModelData> models = _server.Controllers.GetController<EntityController>().Entities
        //     .Select(entity => new
        //     {
        //         ModelComponent = entity.GetComponent<ModelComponent>(),
        //         PositionComponent = entity.GetComponent<PositionComponent>(),
        //         ColliderComponent = entity.GetComponent<ColliderComponent>()
        //     })
        //     .Where(x => x.ModelComponent != null)
        //     .SelectMany(x => _modelDataFactory.Value.CreateFrom(
        //         x.ModelComponent!,
        //         x.PositionComponent ?? throw new NullReferenceException(),
        //         x.ColliderComponent));

        Matrix4x4 viewMatrix = MatrixUtils.GetViewMatrix(new Vector2(0, 0));

        // RenderData renderData = new RenderData(models, viewMatrix);

        // _renderer.Value.Render(renderData);
        _imguiController.Value.Render();
    }

    private void OnResize(Vector2D<int> dimensions)
    {
        _renderer.Value.UpdateDimensions(dimensions);
    }

    private void OnKeyPress(IKeyboard keyboard, Key key, int _)
    {
        _engine.EventQueue.Push(new Event { EventType = EventType.KeyPress, Key = key });
    }

    private void OnKeyRelease(IKeyboard keyboard, Key key, int _)
    {
        _engine.EventQueue.Push(new Event { EventType = EventType.KeyRelease, Key = key });
    }

    #region IDisposable

    ~Viewport()
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

        _window.Dispose();

        _disposed = true;
    }

    #endregion
}