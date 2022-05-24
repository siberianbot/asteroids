using Asteroids.Components;
using Asteroids.Entities;

namespace Asteroids.Behaviors;

public class MovementBehavior : IBehavior
{
    public void Update(UpdateContext context)
    {
        context.DependencyContainer.EntityController.ForEachEntity(entity =>
        {
            MovementComponent? movementComponent = entity.GetComponent<MovementComponent>();
            PositionComponent? positionComponent = entity.GetComponent<PositionComponent>();

            if (movementComponent == null || positionComponent == null)
            {
                return;
            }

            positionComponent.Position += context.Delta * movementComponent.Velocity * movementComponent.Direction;
        });
    }
}