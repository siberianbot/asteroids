using System.Numerics;
using Asteroids.Components;
using Asteroids.Controllers;
using Asteroids.Engine;
using Asteroids.Entities;

namespace Asteroids.Behaviors;

public class EntityCleanupBehavior : IBehavior
{
    private const float MaxDistance = 100f;

    private readonly EntityController _entityController;
    private readonly PlayerController _playerController;

    public EntityCleanupBehavior(EntityController entityController, PlayerController playerController)
    {
        _entityController = entityController;
        _playerController = playerController;
    }

    public void Update(float delta)
    {
        Vector2[] knownPositions = _playerController.Players
            .Where(player => player.Alive)
            .Select(player =>
            {
                return _entityController
                    .GetOwnedEntities<Spaceship>(player)
                    .Single()
                    .GetComponent<PositionComponent>()!
                    .Position;
            })
            .ToArray();

        foreach (Entity entity in _entityController.Entities.Except(_playerController.Players))
        {
            PositionComponent? positionComponent = entity.GetComponent<PositionComponent>();

            if (positionComponent == null)
            {
                continue;
            }

            if (knownPositions.All(position => Vector2.Distance(position, positionComponent.Position) <= MaxDistance))
            {
                continue;
            }

            _entityController.DestroyEntity(entity);
        }
    }
}