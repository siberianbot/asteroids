using System.Numerics;

namespace Asteroids.Entities;

public class Player : Entity
{
    public Player(string name, Vector3 color)
    {
        Name = name;
        Color = color;
        Alive = false;
        Score = 0;
    }

    public string Name { get; }

    public Vector3 Color { get; }

    public long Score { get; set; }

    public bool Alive { get; set; }
}