using System.Numerics;
using Asteroids.Components;

namespace Asteroids.Entities;

public class Bullet : Entity
{
    private readonly Entity _owner;
    private readonly Vector2 _direction;
    private readonly PositionComponent _positionComponent;

    public Bullet(Entity owner, Vector2 position, Vector2 direction)
    {
        _owner = owner;
        _direction = direction;
        _positionComponent = new PositionComponent(position, 0f);

        AddComponent(_positionComponent);
        AddComponent(new ModelComponent(new List<Vector2> { Vector2.Zero }, new Vector3(1f, 1f, 1f)));
    }

    public override void Update(UpdateContext context)
    {
        _positionComponent.Position += _direction * context.Delta;
    }
}