using System.Numerics;
using Asteroids.Entities;

namespace Asteroids.Components;

public class BulletSpawnerComponent : Component, IUpdatableComponent
{
    private readonly Lazy<PositionComponent> _positionComponent;
    private readonly Lazy<MovementComponent> _movementComponent;

    private bool _fire;

    public BulletSpawnerComponent()
    {
        _positionComponent = new Lazy<PositionComponent>(() => Owner.GetComponent<PositionComponent>() ?? throw new ArgumentException());
        _movementComponent = new Lazy<MovementComponent>(() => Owner.GetComponent<MovementComponent>() ?? throw new ArgumentException());
    }

    public void Update(UpdateContext context)
    {
        if (!_fire)
        {
            return;
        }

        Vector2 position = _positionComponent.Value.Position + _movementComponent.Value.Direction;
        Vector2 direction = _movementComponent.Value.Direction;

        context.DependencyContainer.CommandQueue.Push(() => context.DependencyContainer.Spawner.SpawnBullet(Owner, position, direction));

        _fire = false;
    }

    public void Fire()
    {
        _fire = true;
    }
}