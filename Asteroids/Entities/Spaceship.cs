using System.Numerics;
using Asteroids.Components;
using Asteroids.Utils;

namespace Asteroids.Entities;

public class Spaceship : Entity
{
    private static readonly List<Vector2> Model = new List<Vector2>
    {
        new Vector2(0.25f, 0f),
        new Vector2(-0.25f, 0.2f),
        new Vector2(-0.10f, 0f),
        new Vector2(-0.25f, -0.2f),
    };

    private readonly PositionComponent _positionComponent;
    private readonly SpaceshipMovementComponent _movementComponent;

    public Spaceship(Vector2 position, float rotation, Vector3 color)
    {
        _positionComponent = new PositionComponent(position, rotation);
        _movementComponent = new SpaceshipMovementComponent();

        AddComponent(new ModelComponent(Model, color));
        AddComponent(_movementComponent);
        AddComponent(_positionComponent);
    }

    public override void Update(UpdateContext context)
    {
        _movementComponent.Update(context);
        _positionComponent.Position += MathUtils.FromPolar(_positionComponent.Rotation, _movementComponent.Velocity);
    }
}