using System.Numerics;
using Asteroids.Components;
using Asteroids.Controllers;
using Asteroids.Entities;

namespace Asteroids.Behaviors;

public class CameraFollowBehavior : IBehavior
{
    private readonly EntityController _entityController;

    public CameraFollowBehavior(EntityController entityController)
    {
        _entityController = entityController;
    }

    public void Update(float delta)
    {
        foreach (Camera camera in _entityController.Entities.OfType<Camera>())
        {
            if (camera.Owner is not Player { Alive: true } player)
            {
                continue;
            }

            Vector2 spaceshipPosition = _entityController.GetOwnedEntities<Spaceship>(player)
                .Single()
                .GetComponent<PositionComponent>()!
                .Position;

            camera.GetComponent<PositionComponent>()!.Position = spaceshipPosition;
        }
    }
}