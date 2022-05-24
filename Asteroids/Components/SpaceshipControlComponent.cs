using Asteroids.Entities;
using Asteroids.Utils;

namespace Asteroids.Components;

public class SpaceshipControlComponent : Component, IUpdatableComponent
{
    private const float RotationVelocity = MathF.PI;
    private const float MaxVelocity = 7.5f;
    private const float Acceleration = 0.2f;
    private const float Deceleration = 2.5f;

    private float _acceleration;
    private float _rotation;

    private readonly Lazy<PositionComponent> _positionComponent;
    private readonly Lazy<MovementComponent> _movementComponent;

    public SpaceshipControlComponent()
    {
        _positionComponent = new Lazy<PositionComponent>(() => Owner.GetComponent<PositionComponent>() ?? throw new ArgumentException());
        _movementComponent = new Lazy<MovementComponent>(() => Owner.GetComponent<MovementComponent>() ?? throw new ArgumentException());
    }

    public void Update(UpdateContext context)
    {
        _positionComponent.Value.Rotation += context.Delta * _rotation * RotationVelocity;
        _movementComponent.Value.Direction = MathUtils.FromPolar(_positionComponent.Value.Rotation, 1.0f);

        if (_acceleration != 0)
        {
            _movementComponent.Value.Velocity += _acceleration;

            if (MathF.Abs(_movementComponent.Value.Velocity) > MaxVelocity)
            {
                _movementComponent.Value.Velocity = MathF.Sign(_movementComponent.Value.Velocity) * MaxVelocity;
            }
        }
        else if (_movementComponent.Value.Velocity != 0)
        {
            _movementComponent.Value.Velocity -= MathF.Sign(_movementComponent.Value.Velocity) * context.Delta * Deceleration;
        }

        _acceleration = 0;
        _rotation = 0;
    }

    public void Accelerate()
    {
        _acceleration = Acceleration;
    }

    public void Decelerate()
    {
        _acceleration = -Acceleration;
    }

    public void Stop()
    {
        if (MathF.Abs(_movementComponent.Value.Velocity) == 0.0f)
        {
            return;
        }

        _acceleration = MathF.Sign(_movementComponent.Value.Velocity) * -Acceleration;
    }

    public void TurnLeft()
    {
        _rotation = 1.0f;
    }

    public void TurnRight()
    {
        _rotation = -1.0f;
    }
}