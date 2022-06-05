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
    private readonly Spawner _spawner;
    private readonly float _radius;
    private readonly float _maxCooldown;
    private float _cooldown;

    public AsteroidSpawningBehavior(
        PlayerController playerController,
        EntityController entityController,
        Spawner spawner,
        Vars vars)
    {
        _playerController = playerController;
        _entityController = entityController;
        _spawner = spawner;

        _radius = vars.GetVar(Constants.Vars.AsteroidsSpawningRadius, 10.0f);
        _maxCooldown = vars.GetVar(Constants.Vars.AsteroidsSpawningCooldown, 5.0f);
        _cooldown = _maxCooldown;
    }

    public void Update(float delta)
    {
        _cooldown += delta;

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

            _spawner.SpawnAsteroid(
                position + MathUtils.FromPolar(angle, _radius),
                Vector2.Normalize(MathUtils.FromPolar(angle + MathF.PI, _radius)));
        }
    }
}