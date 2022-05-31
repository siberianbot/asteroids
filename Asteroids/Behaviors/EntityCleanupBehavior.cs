using System.Numerics;
using Asteroids.Components;
using Asteroids.Entities;

namespace Asteroids.Behaviors;

public class EntityCleanupBehavior : IBehavior
{
    private const float MaxDistance = 100f;

    public void Update(UpdateContext context)
    {
        Vector2[] knownPositions = context.DependencyContainer.PlayerController.Players
            .Where(player => player.Alive)
            .Select(player => context.DependencyContainer.EntityController
                .GetOwnedEntities<Spaceship>(player)
                .Single()
                .GetComponent<PositionComponent>()!
                .Position)
            .ToArray();

        foreach (Entity entity in context.DependencyContainer.EntityController.Entities
                     .Except(context.DependencyContainer.PlayerController.Players))
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

            context.DependencyContainer.EntityController.Destroy(entity);
        }
    }
}