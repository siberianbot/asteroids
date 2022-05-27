using System.Numerics;
using Asteroids.Entities;
using Asteroids.Utils;

namespace Asteroids.Behaviors;

public class AsteroidSpawningBehavior : IBehavior
{
    private readonly float _radius;
    private readonly float _delay;
    private float _passed;

    public AsteroidSpawningBehavior(float radius, float delay)
    {
        _radius = radius;
        _delay = delay;
        _passed = delay;
    }

    public void Update(UpdateContext context)
    {
        _passed += context.Delta;

        if (_passed < _delay)
        {
            return;
        }

        _passed = 0;

        float angle = Random.Shared.NextSingle() * MathF.Tau;

        context.DependencyContainer.Spawner.SpawnAsteroid(
            MathUtils.FromPolar(angle, _radius),
            Vector2.Normalize(MathUtils.FromPolar(angle + MathF.PI, _radius)));
    }
}