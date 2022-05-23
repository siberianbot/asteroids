namespace Asteroids.Components;

public abstract class MovementComponent : IComponent
{
    public abstract float Velocity { get; protected set; }
}