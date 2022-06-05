using Asteroids.Components;
using Asteroids.Controllers;
using Asteroids.Entities;

namespace Asteroids.Behaviors;

public class MovementBehavior : IBehavior
{
    private readonly EntityController _entityController;

    public MovementBehavior(EntityController entityController)
    {
        _entityController = entityController;
    }

    public void Update(float delta)
    {
        foreach (Entity entity in _entityController.Entities)
        {
            MovementComponent? movementComponent = entity.GetComponent<MovementComponent>();
            PositionComponent? positionComponent = entity.GetComponent<PositionComponent>();

            if (movementComponent == null || positionComponent == null)
            {
                continue;
            }

            positionComponent.Position += delta * movementComponent.Velocity * movementComponent.Direction;
        }
    }
}