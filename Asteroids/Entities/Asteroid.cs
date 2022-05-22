using System.Numerics;
using Asteroids.Utils;

namespace Asteroids.Entities;

public class Asteroid : IEntity
{
    private readonly float _rotationVelocity;
    private readonly Vector2 _direction;
    private readonly List<Vector2> _points;

    private float _rotation;

    public Asteroid(Vector2 position, Vector2 direction, float rotationVelocity, List<Vector2> points)
    {
        Position = position;
        _direction = direction;
        _rotationVelocity = rotationVelocity;
        _points = points;
    }

    public void Update(UpdateContext context)
    {
        _rotation = MathUtils.NormalizeRadian(_rotation + context.Delta * _rotationVelocity);
        Position += context.Delta * _direction;
    }

    public void Render(RenderContext context)
    {
        context.AsteroidRenderer.Render(_points, Position, _rotation);
    }

    public Vector2 Position { get; private set; }
}