using System.Numerics;
using Asteroids.Components;
using Asteroids.Utils;
using Silk.NET.Input;

namespace Asteroids.Entities;

public class Spaceship : Entity
{
    private const float Rotation = MathF.PI / 2;
    private const float RotationVelocity = MathF.PI;
    private const float MaxForwardVelocity = 0.5f;
    private const float ForwardAcceleration = 0.2f;
    private const float MaxBackwardVelocity = -0.25f;
    private const float BackwardAcceleration = -0.1f;
    private const float DecelerationVelocity = 0.1f;

    private static readonly List<Vector2> Model = new List<Vector2>
    {
        new Vector2(0.25f, 0f),
        new Vector2(-0.25f, 0.2f),
        new Vector2(-0.10f, 0f),
        new Vector2(-0.25f, -0.2f),
    };

    private readonly PositionComponent _positionComponent;

    private float _velocity;

    public Spaceship(Vector2 position)
    {
        _positionComponent = new PositionComponent(position, Rotation);
        _velocity = 0f;

        AddComponent(new ModelComponent(Model, new Vector3(0.0f, 0.7f, 0.0f)));
        AddComponent(_positionComponent);
    }

    public override void Update(UpdateContext context)
    {
        if (context.InputController.OnKeyPressed(Key.Left))
        {
            _positionComponent.Rotation = MathUtils.NormalizeRadian(_positionComponent.Rotation + context.Delta * RotationVelocity);
        }

        if (context.InputController.OnKeyPressed(Key.Right))
        {
            _positionComponent.Rotation = MathUtils.NormalizeRadian(_positionComponent.Rotation - context.Delta * RotationVelocity);
        }

        float acceleration = 0.0f;

        if (context.InputController.OnKeyPressed(Key.Up))
        {
            acceleration = ForwardAcceleration;
        }

        if (context.InputController.OnKeyPressed(Key.Down))
        {
            acceleration = BackwardAcceleration;
        }

        if (context.InputController.OnKeyPressed(Key.Z))
        {
            acceleration = _velocity > 0.0f
                ? -ForwardAcceleration
                : _velocity < 0.0f
                    ? -BackwardAcceleration
                    : 0.0f;
        }

        _velocity += acceleration * context.Delta;

        if (_velocity > MaxForwardVelocity)
        {
            _velocity = MaxForwardVelocity;
        }
        else if (_velocity < MaxBackwardVelocity)
        {
            _velocity = MaxBackwardVelocity;
        }
        else if (MathF.Abs(_velocity) > 0f)
        {
            _velocity -= MathF.Sign(_velocity) * DecelerationVelocity * context.Delta;
        }

        _positionComponent.Position += MathUtils.FromPolar(_positionComponent.Rotation, _velocity);
    }
}