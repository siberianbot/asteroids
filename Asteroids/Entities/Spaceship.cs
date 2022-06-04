using System.Numerics;
using Asteroids.Engine;
using Asteroids.Physics;

namespace Asteroids.Entities;

public class Spaceship : Entity, IOwnedEntity
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

    private readonly Player? _owner;

    public Spaceship(Player? owner)
    {
        _owner = owner;
    }

    public override void Create()
    {
        base.Create();

        if (_owner != null)
        {
            _owner.Alive = true;
        }
    }

    public override void Destroy()
    {
        base.Destroy();

        if (_owner != null)
        {
            _owner.Alive = false;
        }
    }

    public Entity? Owner
    {
        get => _owner;
    }
}