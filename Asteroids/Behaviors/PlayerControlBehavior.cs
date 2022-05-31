using Asteroids.Components;
using Asteroids.Engine;
using Asteroids.Entities;
using Silk.NET.Input;

namespace Asteroids.Behaviors;

public class PlayerControlBehavior : IBehavior
{
    public void Update(UpdateContext context)
    {
        InputController inputController = context.DependencyContainer.InputController;

        foreach (Player player in context.DependencyContainer.PlayerController.Players)
        {
            if (!player.Alive)
            {
                continue;
            }

            Spaceship? ownedSpaceship = context.DependencyContainer.EntityController
                .GetOwnedEntities<Spaceship>(player)
                .SingleOrDefault();

            if (ownedSpaceship == null)
            {
                continue;
            }

            SpaceshipControlComponent spaceshipControlComponent = ownedSpaceship.GetComponent<SpaceshipControlComponent>()
                                                                  ?? throw new ArgumentException();
            BulletSpawnerComponent bulletSpawnerComponent = ownedSpaceship.GetComponent<BulletSpawnerComponent>()
                                                            ?? throw new ArgumentException();

            if (inputController.OnKeyPressed(Key.Left))
            {
                spaceshipControlComponent.TurnLeft();
            }

            if (inputController.OnKeyPressed(Key.Right))
            {
                spaceshipControlComponent.TurnRight();
            }

            if (inputController.OnKeyPressed(Key.Z))
            {
                spaceshipControlComponent.Stop();
            }
            else if (inputController.OnKeyPressed(Key.Up))
            {
                spaceshipControlComponent.Accelerate();
            }
            else if (inputController.OnKeyPressed(Key.Down))
            {
                spaceshipControlComponent.Decelerate();
            }

            if (inputController.OnKeyPressed(Key.Space))
            {
                bulletSpawnerComponent.Fire();
            }
        }
    }
}