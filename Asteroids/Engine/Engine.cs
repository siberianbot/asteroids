using System.Numerics;
using Asteroids.Behaviors;
using Asteroids.Components;
using Asteroids.Controllers;
using Asteroids.Entities;
using Asteroids.Physics;
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
    private readonly Lazy<Renderer> _renderer;
    private readonly Lazy<SceneManager> _sceneManager;
    private readonly Lazy<Spawner> _spawner;
    private readonly Lazy<ImGuiController> _imguiController; // <- NOT OUR CONTROLLER
    private readonly Lazy<IInputContext> _inputContext;

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

        _spawner = new Lazy<Spawner>(() => new Spawner(_controllers.GetController<EntityController>(), _controllers.GetController<PlayerController>()));

        _sceneManager = new Lazy<SceneManager>(() => new SceneManager(
            _spawner.Value,
            _controllers.GetController<CameraController>(),
            _controllers.GetController<BehaviorController>(),
            _vars.Value));

        _inputContext = new Lazy<IInputContext>(_window.CreateInput);
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
        _controllers.AddController(new CameraController());
        _controllers.AddController(new BehaviorController(_commandQueue.Value));
        _controllers.AddController(new PlayerController(_commandQueue.Value, _eventQueue.Value));
        _controllers.AddController(new EntityController(_commandQueue.Value, _eventQueue.Value));
        _controllers.AddController(new SceneController(
            _sceneManager.Value,
            _controllers.GetController<EntityController>(),
            _controllers.GetController<CameraController>(),
            _controllers.GetController<BehaviorController>(),
            _controllers.GetController<PlayerController>(),
            _commandQueue.Value));
        _controllers.AddController(new InputController(_inputContext.Value, _commandQueue.Value));

        _controllers.GetController<BehaviorController>().AddBehavior(new DebugBehavior());
        _controllers.GetController<BehaviorController>().AddBehavior(new UIBehavior());
        _controllers.GetController<SceneController>().ChangeScene(Constants.Scenes.PlayableDemo);

        OnResize(_window.Size);
    }

    private void CloseWindow()
    {
        _renderer.Value.Dispose();
    }

    private void OnRender(double _)
    {
        DateTime start = DateTime.UtcNow;

        _renderer.Value.Clear();

        List<ModelData> models = new List<ModelData>();

        // TODO: deal with this mess
        foreach (Entity entity in _controllers.GetController<EntityController>().Entities)
        {
            ModelComponent? modelComponent = entity.GetComponent<ModelComponent>();

            if (modelComponent == null)
            {
                continue;
            }

            PositionComponent positionComponent = entity.GetComponent<PositionComponent>() ?? throw new NullReferenceException();
            ColliderComponent? colliderComponent = entity.GetComponent<ColliderComponent>();

            if (_vars.Value.GetVar(Constants.Vars.Physics_ShowBoundingBox, false) &&
                colliderComponent != null)
            {
                Box2D<float> boundingBox = colliderComponent.BoundingBox;

                ModelData boundingBoxData = new ModelData(
                    new[]
                    {
                        boundingBox.Min.X, boundingBox.Min.Y,
                        boundingBox.Min.X, boundingBox.Max.Y,
                        boundingBox.Max.X, boundingBox.Max.Y,
                        boundingBox.Max.X, boundingBox.Min.Y,
                    },
                    GenerationUtils.GenerateIndicesData(4).ToArray(),
                    4,
                    Constants.Colors.DarkGray,
                    Matrix4x4.CreateTranslation(new Vector3(0.0f, 0.0f, -1.0f)) *
                    Matrix4x4.Identity
                );

                models.Add(boundingBoxData);
            }

            if (_vars.Value.GetVar(Constants.Vars.Physics_ShowCollider, false) &&
                colliderComponent != null)
            {
                foreach (Collider collider in colliderComponent.Colliders)
                {
                    ModelData colliderData = new ModelData(
                        Collider.VerticesOf(collider)
                            .SelectMany(vertices => new[] { vertices.X, vertices.Y })
                            .ToArray(),
                        GenerationUtils.GenerateIndicesData(3).ToArray(),
                        3,
                        Constants.Colors.DarkGray,
                        Matrix4x4.CreateTranslation(0, 0, -1) *
                        Matrix4x4.Identity
                    );

                    models.Add(colliderData);
                }
            }

            ModelData data = new ModelData(
                modelComponent.VerticesData,
                modelComponent.IndicesData,
                modelComponent.Count,
                modelComponent.Color,
                positionComponent.TransformMatrix
            );

            models.Add(data);
        }

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

        _commandQueue.Value.ExecutePending();
        _eventQueue.Value.ExecutePending();

        _imguiController.Value.Update((float)delta);

        UpdateContext context = new UpdateContext
        {
            Delta = (float)delta * _vars.Value.GetVar(Constants.Vars.Engine_TimeMultiplier, 1.0f),
            EngineVars = _engineState.Value,
            Spawner = _spawner.Value,
            CommandQueue = _commandQueue.Value,
            GlobalVars = _vars.Value,
            Controllers = _controllers
        };

        foreach (Entity entity in _controllers.GetController<EntityController>().Entities)
        {
            entity.Update(context);
        }

        foreach (IBehavior behavior in _controllers.GetController<BehaviorController>().Behaviors)
        {
            behavior.Update(context);
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