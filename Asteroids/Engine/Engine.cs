using System.Numerics;
using Asteroids.Behaviors;
using Asteroids.Components;
using Asteroids.Entities;
using Asteroids.Physics;
using Asteroids.Rendering;
using Asteroids.Utils;
using ImGuiNET;
using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace Asteroids.Engine;

public sealed class Engine : IDisposable
{
    private readonly DependencyContainer _dependencyContainer;

    public double UpdateTimeMs { get; private set; }
    public double RenderTimeMs { get; private set; }

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
        _dependencyContainer.BehaviorController.AddBehavior(new DebugBehavior());
        _dependencyContainer.BehaviorController.AddBehavior(new UIBehavior());
        _dependencyContainer.SceneController.ChangeScene(Constants.Scenes.PlayableDemo);

        OnResize(_dependencyContainer.Window.Size);
    }

    private void OnRender(double _)
    {
        DateTime start = DateTime.UtcNow;

        _dependencyContainer.Renderer.Clear();

        List<RenderData> renderList = new List<RenderData>();

        // TODO: deal with this mess
        _dependencyContainer.EntityController.ForEachEntity(entity =>
        {
            ModelComponent? modelComponent = entity.GetComponent<ModelComponent>();

            if (modelComponent == null)
            {
                return;
            }

            PositionComponent positionComponent = entity.GetComponent<PositionComponent>() ?? throw new NullReferenceException();
            ColliderComponent? colliderComponent = entity.GetComponent<ColliderComponent>();

            if (_dependencyContainer.GlobalVars.GetVar(Constants.Vars.Physics_ShowBoundingBox, false) &&
                colliderComponent != null)
            {
                Box2D<float> boundingBox = colliderComponent.BoundingBox;

                RenderData boundingBoxData = new RenderData(
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

                renderList.Add(boundingBoxData);
            }

            if (_dependencyContainer.GlobalVars.GetVar(Constants.Vars.Physics_ShowCollider, false) &&
                colliderComponent != null)
            {
                foreach (Collider collider in colliderComponent.Colliders)
                {
                    RenderData colliderData = new RenderData(
                        Collider.VerticesOf(collider)
                            .SelectMany(vertices => new[] { vertices.X, vertices.Y })
                            .ToArray(),
                        GenerationUtils.GenerateIndicesData(3).ToArray(),
                        3,
                        Constants.Colors.DarkGray,
                        Matrix4x4.CreateTranslation(0, 0, -1) *
                        Matrix4x4.Identity
                    );

                    renderList.Add(colliderData);
                }
            }

            RenderData data = new RenderData(
                modelComponent.VerticesData,
                modelComponent.IndicesData,
                modelComponent.Count,
                modelComponent.Color,
                positionComponent.TransformMatrix
            );

            renderList.Add(data);
        });

        _dependencyContainer.Renderer.Render(renderList);
        _dependencyContainer.ImGuiController.Render();

        RenderTimeMs = (DateTime.UtcNow - start).TotalMilliseconds;
    }

    private void OnUpdate(double delta)
    {
        DateTime start = DateTime.UtcNow;

        _dependencyContainer.ImGuiController.Update((float)delta);
        _dependencyContainer.CommandQueue.ExecutePending();

        UpdateContext context = new UpdateContext
        {
            Delta = (float)delta * _dependencyContainer.GlobalVars.GetVar(Constants.Vars.Engine_TimeMultiplier, 1.0f),
            DependencyContainer = _dependencyContainer
        };

        _dependencyContainer.EntityController.ForEachEntity(entity => entity.Update(context));
        _dependencyContainer.BehaviorController.ForEachBehavior(behavior => behavior.Update(context));

        UpdateTimeMs = (DateTime.UtcNow - start).TotalMilliseconds;
    }

    private void OnResize(Vector2D<int> dimensions)
    {
        _dependencyContainer.Renderer.UpdateDimensions(dimensions);
        _dependencyContainer.ScreenController.ScreenDimensions = new Vector2(dimensions.X, dimensions.Y);
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