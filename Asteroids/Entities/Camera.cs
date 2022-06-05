using System.Numerics;
using Asteroids.Components;
using Asteroids.Engine;
using Asteroids.Utils;

namespace Asteroids.Entities;

public class Camera : Entity
{
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

    public override void Update(float delta)
    {
        base.Update(delta);

        Vector2 position = _entity?.GetComponent<PositionComponent>()?.Position ??
                           _position ??
                           Vector2.Zero;

        ViewMatrix = MatrixUtils.GetViewMatrix(position);
    }

    public Matrix4x4 ViewMatrix { get; private set; }
}