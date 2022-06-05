using Asteroids.Components;
using Asteroids.Engine;
using Asteroids.Utils;

namespace Asteroids.Entities;

public class Asteroid : Entity
{
    private readonly float _rotationVelocity;
    private readonly Lazy<PositionComponent> _positionComponent;

    public Asteroid(float rotationVelocity, float scale)
    {
        _rotationVelocity = rotationVelocity;
        Scale = scale;

        _positionComponent = new Lazy<PositionComponent>(() => GetComponent<PositionComponent>() ?? throw new ArgumentException());
    }

    public float Scale { get; }

    public override void Update(float delta)
    {
        base.Update(delta);

        _positionComponent.Value.Rotation = MathUtils.NormalizeRadian(_positionComponent.Value.Rotation + delta * _rotationVelocity);
    }
}