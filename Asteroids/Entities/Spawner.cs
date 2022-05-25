using System.Numerics;
using Asteroids.Components;
using Asteroids.Engine;
using Asteroids.Utils;

namespace Asteroids.Entities;

public class Spawner
{
    private readonly EntityController _entityController;

    public Spawner(EntityController entityController)
    {
        _entityController = entityController;
    }

    public Asteroid SpawnAsteroid(Vector2 position, Vector2 direction, float? velocity = null, float scale = 1.0f)
    {
        velocity ??= Random.Shared.NextSingle();

        float rotationVelocity = -0.5f + 2 * Random.Shared.NextSingle();

        List<Vector2> points = new List<Vector2>();
        Vector2 center = Vector2.Zero;

        for (int i = 0; i < Constants.AsteroidSpikesCount; i++)
        {
            float angle = MathF.Tau * i / Constants.AsteroidSpikesCount;
            float radius = scale * (0.5f + Random.Shared.NextSingle() / 2);

            Vector2 point = MathUtils.FromPolar(angle, radius);

            center += point;

            points.Add(point);
        }

        center /= Constants.AsteroidSpikesCount;

        for (int i = 0; i < Constants.AsteroidSpikesCount; i++)
        {
            points[i] -= center;
        }

        Asteroid asteroid = new Asteroid(rotationVelocity, scale);
        asteroid.AddComponent(new ModelComponent(points, Constants.Colors.Gray));
        asteroid.AddComponent(new PositionComponent(position, 0f));
        asteroid.AddComponent(new MovementComponent(velocity.Value, direction));
        asteroid.AddComponent(new ColliderComponent(points));

        _entityController.AddEntity(asteroid);

        return asteroid;
    }

    public Spaceship SpawnSpaceship(Vector2 position, Vector3 color)
    {
        float rotation = Random.Shared.NextSingle() * MathF.Tau;

        Spaceship spaceship = new Spaceship();
        spaceship.AddComponent(new ModelComponent(Spaceship.Model, color));
        spaceship.AddComponent(new MovementComponent(0.0f, Vector2.Zero));
        spaceship.AddComponent(new PositionComponent(position, rotation));
        spaceship.AddComponent(new ColliderComponent(Spaceship.Model));
        spaceship.AddComponent(new BulletSpawnerComponent());

        _entityController.AddEntity(spaceship);

        return spaceship;
    }

    public Bullet SpawnBullet(Entity owner, Vector2 position, Vector2 direction)
    {
        Bullet bullet = new Bullet(owner);
        bullet.AddComponent(new ModelComponent(Bullet.Model, Constants.Colors.White));
        bullet.AddComponent(new MovementComponent(10.0f, direction));
        bullet.AddComponent(new PositionComponent(position, 0.0f));
        bullet.AddComponent(new ColliderComponent(Bullet.Model));

        _entityController.AddEntity(bullet);

        return bullet;
    }
}