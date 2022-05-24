using Asteroids.Components;
using Asteroids.Entities;
using Asteroids.Utils;
using Silk.NET.Input;

namespace Asteroids.Behaviors;

public class PlayerBehavior : IBehavior
{
    private const float RotationVelocity = MathF.PI;

    private readonly PositionComponent _positionComponent;
    private readonly SpaceshipMovementComponent _movementComponent;

    public PlayerBehavior(Entity entity)
    {
        _positionComponent = entity.GetComponent<PositionComponent>() ?? throw new ArgumentException();
        _movementComponent = entity.GetComponent<SpaceshipMovementComponent>() ?? throw new ArgumentException();
    }

    public void Update(UpdateContext context)
    {
        if (context.DependencyContainer.InputController.OnKeyPressed(Key.Left))
        {
            _positionComponent.Rotation = MathUtils.NormalizeRadian(_positionComponent.Rotation + context.Delta * RotationVelocity);
        }

        if (context.DependencyContainer.InputController.OnKeyPressed(Key.Right))
        {
            _positionComponent.Rotation = MathUtils.NormalizeRadian(_positionComponent.Rotation - context.Delta * RotationVelocity);
        }

        if (context.DependencyContainer.InputController.OnKeyPressed(Key.Z))
        {
            _movementComponent.Stop();
        }
        else if (context.DependencyContainer.InputController.OnKeyPressed(Key.Up))
        {
            _movementComponent.Accelerate();
        }
        else if (context.DependencyContainer.InputController.OnKeyPressed(Key.Down))
        {
            _movementComponent.Decelerate();
        }
    }
}