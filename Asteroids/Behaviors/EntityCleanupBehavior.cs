using System.Numerics;
using Asteroids.Components;
using Asteroids.Engine;
using Asteroids.Entities;

namespace Asteroids.Behaviors;

public class EntityCleanupBehavior : IBehavior
{
    private const float MaxDistance = 100f;

    public void Update(UpdateContext context)
    {
        Vector2[] knownPositions = context.PlayerController.Players
            .Where(player => player.Alive)
            .Select(player => context.EntityController
                .GetOwnedEntities<Spaceship>(player)
                .Single()
                .GetComponent<PositionComponent>()!
                .Position)
            .ToArray();

        foreach (Entity entity in context.EntityController.Entities.Except(context.PlayerController.Players))
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

            context.EntityController.DestroyEntity(entity);
        }
    }
}