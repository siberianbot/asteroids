using System.Numerics;
using Asteroids.Physics;

namespace Asteroids.Entities;

public class Spaceship : Entity
{
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
}