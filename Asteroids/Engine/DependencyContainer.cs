using Asteroids.Commands;
using Asteroids.Entities;
using Asteroids.Rendering;
using Asteroids.Scenes;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;

namespace Asteroids.Engine;

public class DependencyContainer : IDisposable
{
    private readonly Lazy<BehaviorController> _behaviorController;
    private readonly Lazy<CameraController> _cameraController;
    private readonly Lazy<CommandQueue> _commandQueue;
    private readonly Lazy<EntityController> _entityController;
    private readonly Lazy<ImGuiController> _imguiController;
    private readonly Lazy<InputController> _inputController;
    private readonly Lazy<Renderer> _renderer;
    private readonly Lazy<SceneController> _sceneController;
    private readonly Lazy<SceneManager> _sceneManager;
    private readonly Lazy<Spawner> _spawner;

    public DependencyContainer(Engine engine, IWindow window)
    {
        Engine = engine;
        Window = window;
        GlobalVars = new Vars();

        Lazy<IInputContext> inputContext = new(() => Window.CreateInput());
        Lazy<GL> gl = new(() => Window.CreateOpenGL());

        _behaviorController = new Lazy<BehaviorController>(() => new BehaviorController());
        _commandQueue = new Lazy<CommandQueue>(() => new CommandQueue());
        _entityController = new Lazy<EntityController>(() => new EntityController(CommandQueue));
        _cameraController = new Lazy<CameraController>(() => new CameraController());
        _inputController = new Lazy<InputController>(() => new InputController(inputContext.Value));
        _spawner = new Lazy<Spawner>(() => new Spawner(EntityController));
        _imguiController = new Lazy<ImGuiController>(() => new ImGuiController(gl.Value, window, inputContext.Value));
        _renderer = new Lazy<Renderer>(() => new Renderer(gl.Value, CameraController));
        _sceneManager = new Lazy<SceneManager>(() => new SceneManager(Spawner, CameraController, BehaviorController, GlobalVars,
            EntityController));
        _sceneController = new Lazy<SceneController>(() => new SceneController(SceneManager,
            EntityController, CameraController, BehaviorController, CommandQueue));
    }

    public Engine Engine { get; }

    public IWindow Window { get; }

    public Vars GlobalVars { get; }

    public BehaviorController BehaviorController
    {
        get => _behaviorController.Value;
    }

    public CameraController CameraController
    {
        get => _cameraController.Value;
    }

    public CommandQueue CommandQueue
    {
        get => _commandQueue.Value;
    }

    public EntityController EntityController
    {
        get => _entityController.Value;
    }

    public ImGuiController ImGuiController
    {
        get => _imguiController.Value;
    }

    public InputController InputController
    {
        get => _inputController.Value;
    }

    public Renderer Renderer
    {
        get => _renderer.Value;
    }

    public Spawner Spawner
    {
        get => _spawner.Value;
    }

    public SceneManager SceneManager
    {
        get => _sceneManager.Value;
    }

    public SceneController SceneController
    {
        get => _sceneController.Value;
    }

    #region IDisposable

    private bool _disposed;

    ~DependencyContainer()
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

        Renderer.Dispose();
        Window.Dispose();

        _disposed = true;
    }

    #endregion
}