using System.Numerics;

namespace Asteroids.Entities;

public class Player : Entity
{
    public Player(Vector3 color)
    {
        Color = color;
        Alive = false;
        Score = 0;
    }

    public Vector3 Color { get; }

    public long Score { get; set; }

    public bool Alive { get; set; }
}