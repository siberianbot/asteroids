using Asteroids.Components;
using Asteroids.Controllers;
using Asteroids.Engine;
using Asteroids.Entities;
using Silk.NET.Input;

namespace Asteroids.Behaviors;

// TODO: rework
public class PlayerControlBehavior : IBehavior
{
    public void Update(UpdateContext context)
    {
        InputController inputController = context.Controllers.GetController<InputController>();

        foreach (Player player in context.Controllers.GetController<PlayerController>().Players)
        {
            if (!player.Alive)
            {
                continue;
            }

            Spaceship? ownedSpaceship = context.Controllers.GetController<EntityController>()
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

            if (inputController.IsKeyPressed(Key.Left))
            {
                spaceshipControlComponent.TurnLeft();
            }

            if (inputController.IsKeyPressed(Key.Right))
            {
                spaceshipControlComponent.TurnRight();
            }

            if (inputController.IsKeyPressed(Key.Z))
            {
                spaceshipControlComponent.Stop();
            }
            else if (inputController.IsKeyPressed(Key.Up))
            {
                spaceshipControlComponent.Accelerate();
            }
            else if (inputController.IsKeyPressed(Key.Down))
            {
                spaceshipControlComponent.Decelerate();
            }

            if (inputController.IsKeyPressed(Key.Space))
            {
                bulletSpawnerComponent.Fire();
            }
        }
    }
}