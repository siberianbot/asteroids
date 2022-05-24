using Asteroids.Components;
using Asteroids.Utils;

namespace Asteroids.Entities;

public class Asteroid : Entity
{
    private readonly float _rotationVelocity;
    private readonly Lazy<PositionComponent> _positionComponent;

    public Asteroid(float rotationVelocity)
    {
        _rotationVelocity = rotationVelocity;

        _positionComponent = new Lazy<PositionComponent>(() => GetComponent<PositionComponent>() ?? throw new ArgumentException());
    }

    public override void Update(UpdateContext context)
    {
        base.Update(context);

        _positionComponent.Value.Rotation = MathUtils.NormalizeRadian(_positionComponent.Value.Rotation + context.Delta * _rotationVelocity);
    }
}