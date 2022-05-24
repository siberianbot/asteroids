using System.Numerics;

namespace Asteroids.Entities;

public class Spaceship : Entity
{
    public static readonly List<Vector2> Model = new List<Vector2>
    {
        new Vector2(0.25f, 0f),
        new Vector2(-0.25f, 0.2f),
        new Vector2(-0.10f, 0f),
        new Vector2(-0.25f, -0.2f),
    };
}