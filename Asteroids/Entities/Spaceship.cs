using System.Numerics;
using Asteroids.Components;
using Asteroids.Engine;
using Asteroids.Physics;
using Asteroids.Utils;

namespace Asteroids.Entities;

public class Spaceship : Entity, IOwnedEntity
{
    private const float RotationVelocity = MathF.PI;
    private const float MaxVelocity = 7.5f;
    private const float Acceleration = 0.2f;
    private const float Deceleration = 5.0f;

    public static readonly Vector2[] Model =
    {
        new Vector2(0.25f, 0f),
        new Vector2(-0.25f, 0.2f),
        new Vector2(-0.10f, 0f),
        new Vector2(-0.25f, -0.2f),
    };

    public static readonly Collider[] CollisionModel =
    {
        new Collider(
            new Vector2(0.25f, 0f),
            new Vector2(-0.10f, 0f),
            new Vector2(-0.25f, 0.2f)
        ),
        new Collider(
            new Vector2(0.25f, 0f),
            new Vector2(-0.10f, 0f),
            new Vector2(-0.25f, -0.2f)
        )
    };

    private readonly Lazy<PositionComponent> _positionComponent;
    private readonly Lazy<MovementComponent> _movementComponent;
    private readonly Player? _owner;

    private float _acceleration;
    private float _rotation;

    public Spaceship(Player? owner)
    {
        _owner = owner;
        _positionComponent = new Lazy<PositionComponent>(() => GetComponent<PositionComponent>() ?? throw new ArgumentException());
        _movementComponent = new Lazy<MovementComponent>(() => GetComponent<MovementComponent>() ?? throw new ArgumentException());
    }

    public override void Create()
    {
        base.Create();

        if (_owner != null)
        {
            _owner.Alive = true;
        }
    }

    public override void Update(float delta)
    {
        _positionComponent.Value.Rotation += delta * _rotation * RotationVelocity;
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
            _movementComponent.Value.Velocity -= MathF.Sign(_movementComponent.Value.Velocity) * delta * Deceleration;
        }

        _acceleration = 0;
        _rotation = 0;
    }

    public override void Destroy()
    {
        base.Destroy();

        if (_owner != null)
        {
            _owner.Alive = false;
        }
    }

    public Entity? Owner
    {
        get => _owner;
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