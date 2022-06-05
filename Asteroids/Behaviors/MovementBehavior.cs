using Asteroids.Components;
using Asteroids.Controllers;
using Asteroids.Engine;
using Asteroids.Entities;

namespace Asteroids.Behaviors;

public class MovementBehavior : IBehavior
{
    private readonly EntityController _entityController;

    public MovementBehavior(EntityController entityController)
    {
        _entityController = entityController;
    }

    public void Update(UpdateContext context)
    {
        foreach (Entity entity in _entityController.Entities)
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