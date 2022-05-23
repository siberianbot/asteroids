using System.Numerics;
using Asteroids.Components;
using Asteroids.Utils;

namespace Asteroids.Entities;

public class Asteroid : Entity
{
    private readonly float _rotationVelocity;
    private readonly Vector2 _direction;

    private readonly PositionComponent _positionComponent;
    private readonly ConstantMovementComponent _movementComponent;

    public Asteroid(Vector2 position, Vector2 direction, float velocity, float rotationVelocity, List<Vector2> points)
    {
        _direction = direction;
        _rotationVelocity = rotationVelocity;

        _positionComponent = new PositionComponent(position, 0f);
        _movementComponent = new ConstantMovementComponent(velocity);

        AddComponent(new ModelComponent(points, new Vector3(0.7f, 0.7f, 0.7f)));
        AddComponent(_positionComponent);
        AddComponent(_movementComponent);
    }

    public override void Update(UpdateContext context)
    {
        _positionComponent.Rotation = MathUtils.NormalizeRadian(_positionComponent.Rotation + context.Delta * _rotationVelocity);
        _positionComponent.Position += context.Delta * _movementComponent.Velocity * _direction;
    }
}