using System.Numerics;
using Asteroids.Physics;

namespace Asteroids.Entities;

public class Bullet : Entity
{
    public static readonly Vector2[] Model =
    {
        new Vector2(0.0f, 0.0f),
        new Vector2(0.0f, -0.05f)
    };

    public static readonly Collider[] CollisionModel =
    {
        new Collider(Vector2.Zero, Vector2.Zero, Vector2.Zero)
    };

    public Bullet(Entity owner)
    {
        Owner = owner;
    }

    public Entity Owner { get; }
}