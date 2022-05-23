namespace Asteroids.Components;

public sealed class ConstantMovementComponent : MovementComponent
{
    public ConstantMovementComponent(float velocity)
    {
        Velocity = velocity;
    }

    public override float Velocity { get; protected set; }
}