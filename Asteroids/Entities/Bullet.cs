using System.Numerics;

namespace Asteroids.Entities;

public class Bullet : Entity
{
    public static readonly List<Vector2> Model = new List<Vector2>
    {
        new Vector2(0.0f, 0.0f),
        new Vector2(0.0f, 0.05f)
    };

    public Bullet(Entity owner)
    {
        Owner = owner;
    }

    public Entity Owner { get; }
}