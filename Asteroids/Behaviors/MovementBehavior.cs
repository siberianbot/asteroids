using Asteroids.Components;
using Asteroids.Engine;
using Asteroids.Entities;

namespace Asteroids.Behaviors;

public class MovementBehavior : IBehavior
{
    public void Update(UpdateContext context)
    {
        foreach (Entity entity in context.EntityController.Entities)
        {
            MovementComponent? movementComponent = entity.GetComponent<MovementComponent>();
            PositionComponent? positionComponent = entity.GetComponent<PositionComponent>();

            if (movementComponent == null || positionComponent == null)
            {
                continue;
            }

            positionComponent.Position += context.Delta * movementComponent.Velocity * movementComponent.Direction;
        }
    }
}