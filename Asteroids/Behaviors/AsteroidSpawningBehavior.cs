using System.Numerics;
using Asteroids.Components;
using Asteroids.Controllers;
using Asteroids.Engine;
using Asteroids.Entities;
using Asteroids.Utils;

namespace Asteroids.Behaviors;

public class AsteroidSpawningBehavior : IBehavior
{
    private readonly PlayerController _playerController;
    private readonly EntityController _entityController;
    private readonly float _radius;
    private readonly float _maxCooldown;
    private float _cooldown;

    public AsteroidSpawningBehavior(PlayerController playerController, EntityController entityController, float radius, float maxCooldown)
    {
        _playerController = playerController;
        _entityController = entityController;
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

        foreach (Player player in _playerController.Players)
        {
            if (!player.Alive)
            {
                continue;
            }

            Vector2 position = _entityController.GetOwnedEntities<Spaceship>(player)
                .Single()
                .GetComponent<PositionComponent>()!
                .Position;

            float angle = Random.Shared.NextSingle() * MathF.Tau;

            context.Spawner.SpawnAsteroid(
                position + MathUtils.FromPolar(angle, _radius),
                Vector2.Normalize(MathUtils.FromPolar(angle + MathF.PI, _radius)));
        }
    }
}