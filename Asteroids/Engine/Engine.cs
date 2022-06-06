using System.Numerics;
using Asteroids.Behaviors;
using Asteroids.Components;
using Asteroids.Controllers;
using Asteroids.Entities;
using Asteroids.Rendering;
using Asteroids.Scenes;
using Asteroids.Utils;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;

namespace Asteroids.Engine;

public sealed class Engine : IDisposable
{
    private readonly IWindow _window;

    private readonly Lazy<CommandQueue> _commandQueue;
    private readonly Lazy<EventQueue> _eventQueue;
    private readonly Lazy<EngineVars> _engineState;
    private readonly Lazy<Vars> _vars;
    private readonly Lazy<ModelDataFactory> _modelDataFactory;
    private readonly Lazy<Renderer> _renderer;
    private readonly Lazy<SceneManager> _sceneManager;
    private readonly Lazy<Spawner> _spawner;
    private readonly Lazy<ImGuiController> _imguiController; // <- NOT OUR CONTROLLER
    private readonly Lazy<IInputContext> _inputContext;
    private readonly Lazy<BehaviorFactory> _behaviorFactory;

    private readonly ControllersCollection _controllers = new ControllersCollection();

    public Engine()
    {
        _window = Window.Create(WindowOptions.Default);
        _window.Load += InitWindow;
        _window.Closing += CloseWindow;
        _window.Render += OnRender;
        _window.Update += OnUpdate;
        _window.FramebufferResize += OnResize;

        _commandQueue = new Lazy<CommandQueue>(() => new CommandQueue());
        _eventQueue = new Lazy<EventQueue>(() => new EventQueue());
        _engineState = new Lazy<EngineVars>();
        _vars = new Lazy<Vars>(() => new Vars());

        _spawner = new Lazy<Spawner>(() => new Spawner(
            _controllers.GetController<EntityController>(),
            _controllers.GetController<PlayerController>()));

        _behaviorFactory = new Lazy<BehaviorFactory>(() => new BehaviorFactory(
            _controllers,
            _commandQueue.Value,
            _engineState.Value,
            _eventQueue.Value,
            _spawner.Value,
            _vars.Value));

        _sceneManager = new Lazy<SceneManager>(() => new SceneManager(
            _spawner.Value,
            _behaviorFactory.Value,
            _controllers));

        _inputContext = new Lazy<IInputContext>(_window.CreateInput);
        _modelDataFactory = new Lazy<ModelDataFactory>(() => new ModelDataFactory(_vars.Value));
        Lazy<GL> gl = new Lazy<GL>(_window.CreateOpenGL);
        _renderer = new Lazy<Renderer>(() => new Renderer(gl.Value));
        _imguiController = new Lazy<ImGuiController>(() => new ImGuiController(gl.Value, _window, _inputContext.Value));
    }

    public void Run()
    {
        _window.Run();
    }

    private void InitWindow()
    {
        _controllers.AddController(new CameraController(_eventQueue.Value));
        _controllers.AddController(new BehaviorController(_commandQueue.Value, _eventQueue.Value));
        _controllers.AddController(new PlayerController(_commandQueue.Value, _eventQueue.Value));
        _controllers.AddController(new EntityController(_commandQueue.Value, _eventQueue.Value));
        _controllers.AddController(new SceneController(_sceneManager.Value, _commandQueue.Value, _eventQueue.Value, _vars.Value));
        _controllers.AddController(new InputController(_inputContext.Value, _eventQueue.Value));

        _controllers.InitializeAll();

        _controllers.GetController<BehaviorController>().AddBehavior(_behaviorFactory.Value.CreateDebugBehavior());
        _controllers.GetController<BehaviorController>().AddBehavior(_behaviorFactory.Value.CreateUIBehavior());
        _controllers.GetController<SceneController>().ChangeScene(Constants.Scenes.PlayableDemo);

        OnResize(_window.Size);
    }

    private void CloseWindow()
    {
        _controllers.TerminateAll();
        _renderer.Value.Dispose();
    }

    private void OnRender(double _)
    {
        DateTime start = DateTime.UtcNow;

        _renderer.Value.Clear();

        IEnumerable<ModelData> models = _controllers.GetController<EntityController>().Entities
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

        Matrix4x4 viewMatrix = _controllers.GetController<CameraController>().CurrentCamera?.ViewMatrix ??
                               MatrixUtils.GetViewMatrix(new Vector2(0, 0));

        RenderData renderData = new RenderData(models, viewMatrix);

        _renderer.Value.Render(renderData);
        _imguiController.Value.Render();

        _engineState.Value.RenderTimeMs = (DateTime.UtcNow - start).TotalMilliseconds;
    }

    private void OnUpdate(double delta)
    {
        DateTime start = DateTime.UtcNow;

        _eventQueue.Value.ExecutePending();
        _commandQueue.Value.ExecutePending();

        _imguiController.Value.Update((float)delta);

        float targetDelta = (float)delta * _vars.Value.GetVar(Constants.Vars.EngineTimeMultiplier, 1.0f);

        foreach (Entity entity in _controllers.GetController<EntityController>().Entities)
        {
            entity.Update(targetDelta);
        }

        foreach (IBehavior behavior in _controllers.GetController<BehaviorController>().Behaviors)
        {
            behavior.Update(targetDelta);
        }

        _engineState.Value.UpdateTimeMs = (DateTime.UtcNow - start).TotalMilliseconds;
    }

    private void OnResize(Vector2D<int> dimensions)
    {
        _renderer.Value.UpdateDimensions(dimensions);
        _engineState.Value.ScreenDimensions = new Vector2(dimensions.X, dimensions.Y);
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

        _window.Dispose();

        _disposed = true;
    }

    #endregion
}