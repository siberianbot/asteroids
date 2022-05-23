using System.Numerics;
using Asteroids.Components;
using Asteroids.Utils;
using Silk.NET.Input;

namespace Asteroids.Entities;

public class Spaceship : Entity
{
    private const float Rotation = MathF.PI / 2;
    private const float RotationVelocity = MathF.PI;
    private const float MaxVelocity = 0.5f;
    private const float Acceleration = 0.2f;
    private const float Deceleration = 0.1f;

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

        float absVelocity = MathF.Abs(_velocity);
        int sign = MathF.Sign(_velocity);

        _velocity += context.Delta * CalculateAcceleration(context, absVelocity, sign);

        switch (absVelocity)
        {
            case > MaxVelocity:
                _velocity = sign * MaxVelocity;
                break;
            case > 0f:
                _velocity -= MathF.Sign(_velocity) * Deceleration * context.Delta;
                break;
        }

        _positionComponent.Position += MathUtils.FromPolar(_positionComponent.Rotation, _velocity);
    }

    private static float CalculateAcceleration(UpdateContext context, float absVelocity, int sign)
    {
        if (context.InputController.OnKeyPressed(Key.Z) && absVelocity > 0.0f)
        {
            return sign * -Acceleration;
        }

        if (context.InputController.OnKeyPressed(Key.Up))
        {
            return Acceleration;
        }

        if (context.InputController.OnKeyPressed(Key.Down))
        {
            return -Acceleration;
        }

        return 0.0f;
    }
}