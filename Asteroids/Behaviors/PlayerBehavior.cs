using Asteroids.Components;
using Asteroids.Entities;
using Silk.NET.Input;

namespace Asteroids.Behaviors;

public class PlayerBehavior : IBehavior
{
    private readonly SpaceshipControlComponent _spaceshipControlComponent;
    private readonly BulletSpawnerComponent _bulletSpawnerComponent;

    public PlayerBehavior(Entity entity)
    {
        _spaceshipControlComponent = entity.GetComponent<SpaceshipControlComponent>() ?? throw new ArgumentException();
        _bulletSpawnerComponent = entity.GetComponent<BulletSpawnerComponent>() ?? throw new ArgumentException();
    }

    public void Update(UpdateContext context)
    {
        if (context.DependencyContainer.InputController.OnKeyPressed(Key.Left))
        {
            _spaceshipControlComponent.TurnLeft();
        }

        if (context.DependencyContainer.InputController.OnKeyPressed(Key.Right))
        {
            _spaceshipControlComponent.TurnRight();
        }

        if (context.DependencyContainer.InputController.OnKeyPressed(Key.Z))
        {
            _spaceshipControlComponent.Stop();
        }
        else if (context.DependencyContainer.InputController.OnKeyPressed(Key.Up))
        {
            _spaceshipControlComponent.Accelerate();
        }
        else if (context.DependencyContainer.InputController.OnKeyPressed(Key.Down))
        {
            _spaceshipControlComponent.Decelerate();
        }

        if (context.DependencyContainer.InputController.OnKeyPressed(Key.Space))
        {
            _bulletSpawnerComponent.Fire();
        }
    }
}