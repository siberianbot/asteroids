using System.Numerics;
using Asteroids.Components;
using Asteroids.Engine;
using Asteroids.Physics;
using Asteroids.Utils;

namespace Asteroids.Entities;

public class Spawner
{
    private readonly EntityController _entityController;

    public Spawner(EntityController entityController)
    {
        _entityController = entityController;
    }

    public Asteroid SpawnAsteroid(Vector2 position, Vector2 direction, float? velocity = null, float? scale = null)
    {
        velocity ??= Random.Shared.NextSingle();
        scale ??= 0.25f + 0.75f * Random.Shared.NextSingle();

        float rotationVelocity = -MathF.PI + MathF.Tau * Random.Shared.NextSingle();

        List<Vector2> points = new List<Vector2>();
        Vector2 center = Vector2.Zero;

        for (int i = 0; i < Constants.Asteroids.SpikesCount; i++)
        {
            float angle = MathF.Tau * i / Constants.Asteroids.SpikesCount;
            float radius = scale.Value * (0.5f + 0.5f * Random.Shared.NextSingle());

            Vector2 point = MathUtils.FromPolar(angle, radius);

            center += point;

            points.Add(point);
        }

        center /= Constants.Asteroids.SpikesCount;

        for (int i = 0; i < Constants.Asteroids.SpikesCount; i++)
        {
            points[i] -= center;
        }

        List<Collider> colliders = new List<Collider>();

        for (int idx = 0; idx < points.Count; idx++)
        {
            int nextIdx = idx + 1 == points.Count ? 0 : idx + 1;

            Collider collider = new Collider(
                Vector2.Zero,
                points[idx],
                points[nextIdx]
            );

            colliders.Add(collider);
        }

        Asteroid asteroid = new Asteroid(rotationVelocity, scale.Value);
        asteroid.AddComponent(new ModelComponent(points, Constants.Colors.Gray));
        asteroid.AddComponent(new ColliderComponent(colliders));
        asteroid.AddComponent(new PositionComponent(position, 0f));
        asteroid.AddComponent(new MovementComponent(velocity.Value, direction));

        _entityController.AddEntity(asteroid);

        return asteroid;
    }

    public Spaceship SpawnSpaceship(Vector2 position, Vector3 color)
    {
        float rotation = Random.Shared.NextSingle() * MathF.Tau;

        Spaceship spaceship = new Spaceship();
        spaceship.AddComponent(new ModelComponent(Spaceship.Model, color));
        spaceship.AddComponent(new ColliderComponent(Spaceship.CollisionModel));
        spaceship.AddComponent(new MovementComponent(0.0f, Vector2.Zero));
        spaceship.AddComponent(new PositionComponent(position, rotation));
        spaceship.AddComponent(new BulletSpawnerComponent());

        _entityController.AddEntity(spaceship);

        return spaceship;
    }

    public Bullet SpawnBullet(Entity owner, Vector2 position, Vector2 direction)
    {
        Bullet bullet = new Bullet(owner);
        bullet.AddComponent(new ModelComponent(Bullet.Model, Constants.Colors.White));
        bullet.AddComponent(new ColliderComponent(Bullet.CollisionModel));
        bullet.AddComponent(new MovementComponent(10.0f, direction));
        bullet.AddComponent(new PositionComponent(position, 0.0f));

        _entityController.AddEntity(bullet);

        return bullet;
    }
}