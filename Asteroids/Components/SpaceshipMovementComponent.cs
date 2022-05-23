using Asteroids.Entities;

namespace Asteroids.Components;

public sealed class SpaceshipMovementComponent : MovementComponent, IUpdatableComponent
{
    private const float MaxVelocity = 0.5f;
    private const float Acceleration = 0.2f;
    private const float Deceleration = 0.1f;

    private float _acceleration;

    public override float Velocity { get; protected set; }

    public void Update(UpdateContext context)
    {
        Velocity += context.Delta * _acceleration;

        float absVelocity = MathF.Abs(Velocity);
        int sign = MathF.Sign(Velocity);

        switch (absVelocity)
        {
            case > MaxVelocity:
                Velocity = sign * MaxVelocity;
                break;

            case > 0.0f:
                Velocity -= sign * context.Delta * Deceleration;
                break;
        }

        _acceleration = 0;
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
        if (MathF.Abs(Velocity) == 0.0f)
        {
            return;
        }

        _acceleration = MathF.Sign(Velocity) * -Acceleration;
    }
}