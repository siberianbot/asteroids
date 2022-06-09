using System.Numerics;
using Asteroids.Client.Controllers;
using Asteroids.Client.UI;
using Asteroids.Components;
using Asteroids.Controllers;
using Asteroids.Engine;
using Asteroids.Rendering;
using Asteroids.Utils;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;

namespace Asteroids.Client;

public class Client : IDisposable
{
    private readonly Server.Server _server;
    private readonly EventQueue _eventQueue = new EventQueue();
    private readonly Vars _vars = new Vars();

    private readonly IWindow _window;
    private bool _disposed;

    private readonly Lazy<IInputContext> _inputContext;
    private readonly Lazy<ModelDataFactory> _modelDataFactory;
    private readonly Lazy<Renderer> _renderer;
    private readonly Lazy<ImGuiController> _imguiController;
    private readonly ControllersCollection _controllers = new ControllersCollection();

    private readonly List<IClientUI> _clientUIs = new List<IClientUI>();

    public Client(Server.Server server)
    {
        _server = server;

        _window = Window.Create(WindowOptions.Default);
        _window.Load += OnLoadWindow;
        _window.Closing += OnClosingWindow;
        _window.Render += OnRender;
        _window.Update += OnUpdate;
        _window.FramebufferResize += OnResize;

        _inputContext = new Lazy<IInputContext>(_window.CreateInput);
        _modelDataFactory = new Lazy<ModelDataFactory>(() => new ModelDataFactory(_vars));
        Lazy<GL> gl = new Lazy<GL>(_window.CreateOpenGL);
        _renderer = new Lazy<Renderer>(() => new Renderer(gl.Value));
        _imguiController = new Lazy<ImGuiController>(() => new ImGuiController(gl.Value, _window, _inputContext.Value));
    }

    public void Run()
    {
        _window.Run();
    }

    private void OnLoadWindow()
    {
        _controllers.AddController(new InputController(_inputContext.Value, _eventQueue));
        _controllers.InitializeAll();

        _clientUIs.Add(new DebugUI(_eventQueue, _vars));
        // _clientUIs.Add(new MenuUI(_window));

        foreach (IClientUI clientUI in _clientUIs)
        {
            clientUI.Initialize();
        }

        _renderer.Value.UpdateDimensions(_window.Size);
    }

    private void OnClosingWindow()
    {
        foreach (IClientUI clientUI in _clientUIs)
        {
            clientUI.Terminate();
        }

        _controllers.TerminateAll();

        // TODO: it is incorrect to dispose renderer on window close
        _renderer.Value.Dispose();
    }

    private void OnUpdate(double delta)
    {
        _eventQueue.ExecutePending();

        _imguiController.Value.Update((float)delta);

        foreach (IClientUI clientUI in _clientUIs)
        {
            clientUI.Update();
        }
    }

    private void OnRender(double delta)
    {
        _renderer.Value.Clear();

        IEnumerable<ModelData> models = _server.Controllers.GetController<EntityController>().Entities
            .Select(entity => new
            {
                ModelComponent = entity.GetComponent<ModelComponent>(),
                PositionComponent = entity.GetComponent<PositionComponent>(),
                ColliderComponent = entity.GetComponent<ColliderComponent>()
            })
            .Where(x => x.ModelComponent != null)
            .SelectMany(x => _modelDataFactory.Value.CreateFrom(
                x.ModelComponent!,
                x.PositionComponent ?? throw new NullReferenceException(),
                x.ColliderComponent));

        Matrix4x4 viewMatrix = MatrixUtils.GetViewMatrix(new Vector2(0, 0));

        RenderData renderData = new RenderData(models, viewMatrix);

        _renderer.Value.Render(renderData);
        _imguiController.Value.Render();
    }

    private void OnResize(Vector2D<int> dimensions)
    {
        _renderer.Value.UpdateDimensions(dimensions);
    }

    #region IDisposable

    ~Client()
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