using System.Numerics;

namespace Asteroids.Components;

public class MovementComponent : Component
{
    public MovementComponent(float velocity, Vector2 direction)
    {
        Velocity = velocity;
        Direction = direction;
    }

    public float Velocity { get; set; }

    public Vector2 Direction { get; set; }
}