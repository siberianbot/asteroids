using Asteroids.Entities;

namespace Asteroids.Physics;

public class Collision
{
    public Entity Left { get; }

    public Entity Right { get; }

    public Collision(Entity left, Entity right)
    {
        Left = left;
        Right = right;
    }
}