using System.Numerics;
using Asteroids.Components;
using Asteroids.Entities;
using Asteroids.Utils;

namespace Asteroids.Behaviors;

public class AsteroidSpawningBehavior : IBehavior
{
    private readonly float _radius;
    private readonly float _maxCooldown;
    private float _cooldown;

    public AsteroidSpawningBehavior(float radius, float maxCooldown)
    {
        _radius = radius;
        _maxCooldown = maxCooldown;
        _cooldown = maxCooldown;
    }

    public void Update(UpdateContext context)
    {
        _cooldown += context.Delta;

        if (_cooldown < _maxCooldown)
        {
            return;
        }

        _cooldown = 0;

        foreach (Player player in context.DependencyContainer.PlayerController.Players)
        {
            if (!player.Alive)
            {
                continue;
            }

            Vector2 position = context.DependencyContainer.EntityController
                .GetOwnedEntities<Spaceship>(player)
                .Single()
                .GetComponent<PositionComponent>()!
                .Position;

            float angle = Random.Shared.NextSingle() * MathF.Tau;

            context.DependencyContainer.Spawner.SpawnAsteroid(
                position + MathUtils.FromPolar(angle, _radius),
                Vector2.Normalize(MathUtils.FromPolar(angle + MathF.PI, _radius)));
        }
    }
}