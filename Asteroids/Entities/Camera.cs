using System.Numerics;
using Asteroids.Components;

namespace Asteroids.Entities;

public class Camera : Entity
{
    private const float Near = 0.00001f;
    private const float Far = 20.00000f;
    private const float Fov = MathF.PI / 2;
    private static readonly Vector3 Forward = new Vector3(0f, 0f, -1f);
    private static readonly Vector3 Up = new Vector3(0f, 1f, 0f);

    private readonly Entity? _entity;
    private readonly Vector2? _position;

    public Camera(Entity entity)
    {
        _entity = entity;
    }

    public Camera(Vector2 position)
    {
        _position = position;
    }

    public override void Update(UpdateContext context)
    {
        base.Update(context);

        Vector2 position = _entity?.GetComponent<PositionComponent>()?.Position ??
                           _position ??
                           Vector2.Zero;

        ViewMatrix = Matrix4x4.CreateLookAt(new Vector3(position, 5.0f), new Vector3(position, 5.0f) + Forward, Up);
        ProjectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(Fov, context.DependencyContainer.ScreenController.ScreenAspectRatio, Near, Far);
    }

    public Matrix4x4 ViewMatrix { get; private set; }

    public Matrix4x4 ProjectionMatrix { get; private set; }
}