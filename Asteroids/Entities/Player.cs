using System.Numerics;
using Asteroids.Engine;

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

    public override void Destroy(DestroyContext context)
    {
        base.Destroy(context);

        // TODO: removal of player should not be performed by entity itself
        context.PlayerController.RemovePlayer(this);
    }
}