using System.Numerics;
using Asteroids.Components;
using Asteroids.Utils;

namespace Asteroids.Entities;

public class Asteroid : Entity
{
    private readonly float _rotationVelocity;
    private readonly Vector2 _direction;

    private readonly PositionComponent _positionComponent;

    public Asteroid(Vector2 position, Vector2 direction, float rotationVelocity, List<Vector2> points)
    {
        _direction = direction;
        _rotationVelocity = rotationVelocity;

        _positionComponent = new PositionComponent(position, 0f);
        
        AddComponent(new ModelComponent(points, new Vector3(0.7f, 0.7f, 0.7f)));
        AddComponent(_positionComponent);
    }

    public override void Update(UpdateContext context)
    {
        _positionComponent.Rotation = MathUtils.NormalizeRadian(_positionComponent.Rotation + context.Delta * _rotationVelocity);
        _positionComponent.Position += context.Delta * _direction;
    }
}