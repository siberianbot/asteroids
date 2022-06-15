using System.Numerics;
using Asteroids.Components;
using Asteroids.Controllers;
using Asteroids.Entities;

namespace Asteroids.Behaviors;

public class BulletSpawningBehavior : IBehavior
{
    private const float MaxCooldown = 0.5f;

    private readonly EntityController _entityController;
    private readonly Spawner _spawner;
    private readonly Dictionary<Entity, float> _cooldown = new Dictionary<Entity, float>();

    public BulletSpawningBehavior(EntityController entityController, Spawner spawner)
    {
        _entityController = entityController;
        _spawner = spawner;
    }

    public void Update(float delta)
    {
        foreach (Entity entity in _entityController.Entities)
        {
            BulletSpawnerComponent? bulletSpawnerComponent = entity.GetComponent<BulletSpawnerComponent>();

            if (bulletSpawnerComponent == null)
            {
                continue;
            }

            if (!_cooldown.ContainsKey(entity))
            {
                _cooldown[entity] = MaxCooldown;
            }

            _cooldown[entity] += delta;

            if (!bulletSpawnerComponent.Fire || _cooldown[entity] < MaxCooldown)
            {
                continue;
            }

            _cooldown[entity] = 0;
            bulletSpawnerComponent.Fire = false;

            PositionComponent positionComponent = entity.GetComponent<PositionComponent>() ?? throw new ArgumentException();
            MovementComponent movementComponent = entity.GetComponent<MovementComponent>() ?? throw new ArgumentException();

            Vector2 position = positionComponent.Position + movementComponent.Direction * 0.3f;
            Vector2 direction = movementComponent.Direction;

            _spawner.SpawnBullet(entity, position, direction);
        }
    }
}