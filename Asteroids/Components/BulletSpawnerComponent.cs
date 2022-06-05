using System.Numerics;
using Asteroids.Engine;
using Asteroids.Entities;

namespace Asteroids.Components;

public class BulletSpawnerComponent : Component, IUpdatableComponent
{
    private const float Cooldown = 0.5f;

    private readonly CommandQueue _commandQueue;
    private readonly Spawner _spawner;
    private readonly Lazy<PositionComponent> _positionComponent;
    private readonly Lazy<MovementComponent> _movementComponent;

    private float _offset;
    private bool _fire;

    public BulletSpawnerComponent(CommandQueue commandQueue, Spawner spawner)
    {
        _commandQueue = commandQueue;
        _spawner = spawner;
        _positionComponent = new Lazy<PositionComponent>(() => Owner.GetComponent<PositionComponent>() ?? throw new ArgumentException());
        _movementComponent = new Lazy<MovementComponent>(() => Owner.GetComponent<MovementComponent>() ?? throw new ArgumentException());
    }

    public void Update(float delta)
    {
        _offset += delta;

        if (!_fire || _offset <= Cooldown)
        {
            return;
        }

        Vector2 position = _positionComponent.Value.Position + _movementComponent.Value.Direction;
        Vector2 direction = _movementComponent.Value.Direction;

        _commandQueue.Push(() => _spawner.SpawnBullet(Owner, position, direction));

        _fire = false;
        _offset = 0.0f;
    }

    public void Fire()
    {
        _fire = true;
    }
}