using System.Numerics;
using Asteroids.Behaviors;
using Asteroids.Components;
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
        _dependencyContainer.BehaviorController.AddBehavior(new DebugBehavior());
        _dependencyContainer.SceneController.ChangeScene(Constants.Scenes.Testbed);

        OnResize(_dependencyContainer.Window.Size);
    }

    private void OnRender(double delta)
    {
        _dependencyContainer.Renderer.Clear();

        List<RenderData> renderList = new List<RenderData>();

        _dependencyContainer.EntityController.ForEachEntity(entity =>
        {
            ModelComponent modelComponent = entity.GetComponent<ModelComponent>() ?? throw new NullReferenceException();
            PositionComponent positionComponent = entity.GetComponent<PositionComponent>() ?? throw new NullReferenceException();

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
    }

    private void OnUpdate(double delta)
    {
        _dependencyContainer.CommandQueue.ExecutePending();

        UpdateContext context = new UpdateContext
        {
            Delta = (float)delta,
            DependencyContainer = _dependencyContainer
        };

        _dependencyContainer.EntityController.ForEachEntity(entity => entity.Update(context));
        _dependencyContainer.BehaviorController.ForEachBehavior(behavior => behavior.Update(context));
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