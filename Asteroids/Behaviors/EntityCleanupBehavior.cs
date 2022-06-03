using System.Numerics;
using Asteroids.Components;
using Asteroids.Controllers;
using Asteroids.Engine;
using Asteroids.Entities;

namespace Asteroids.Behaviors;

public class EntityCleanupBehavior : IBehavior
{
    private const float MaxDistance = 100f;

    public void Update(UpdateContext context)
    {
        EntityController entityController = context.Controllers.GetController<EntityController>();
        PlayerController playerController = context.Controllers.GetController<PlayerController>();

        Vector2[] knownPositions = playerController.Players
            .Where(player => player.Alive)
            .Select(player =>
            {
                return entityController
                    .GetOwnedEntities<Spaceship>(player)
                    .Single()
                    .GetComponent<PositionComponent>()!
                    .Position;
            })
            .ToArray();

        foreach (Entity entity in entityController.Entities.Except(playerController.Players))
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

            entityController.DestroyEntity(entity);
        }
    }
}