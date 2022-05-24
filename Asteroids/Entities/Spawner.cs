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

    public Asteroid SpawnAsteroid(Vector2 position, Vector2 direction)
    {
        float velocity = Random.Shared.NextSingle();
        float rotationVelocity = Random.Shared.NextSingle();

        List<Vector2> points = new List<Vector2>();
        Vector2 center = Vector2.Zero;

        for (int i = 0; i < Constants.AsteroidSpikesCount; i++)
        {
            float angle = MathF.Tau * i / Constants.AsteroidSpikesCount;
            float radius = 0.5f + Random.Shared.NextSingle() / 2;

            Vector2 point = MathUtils.FromPolar(angle, radius);

            center += point;

            points.Add(point);
        }

        center /= Constants.AsteroidSpikesCount;

        for (int i = 0; i < Constants.AsteroidSpikesCount; i++)
        {
            points[i] -= center;
        }

        Asteroid asteroid = new Asteroid(rotationVelocity);
        asteroid.AddComponent(new ModelComponent(points, new Vector3(0.7f, 0.7f, 0.7f)));
        asteroid.AddComponent(new PositionComponent(position, 0f));
        asteroid.AddComponent(new MovementComponent(velocity, direction));

        _entityController.AddEntity(asteroid);

        return asteroid;
    }

    public Spaceship SpawnSpaceship(Vector2 position, Vector3? color)
    {
        float rotation = Random.Shared.NextSingle() * MathF.Tau;

        if (color == null)
        {
            int idx = Random.Shared.Next(Constants.Colors.Length);

            color = Constants.Colors[idx];
        }

        Spaceship spaceship = new Spaceship();
        spaceship.AddComponent(new ModelComponent(Spaceship.Model, color.Value));
        spaceship.AddComponent(new MovementComponent(0.0f, Vector2.Zero));
        spaceship.AddComponent(new PositionComponent(position, rotation));

        _entityController.AddEntity(spaceship);

        return spaceship;
    }

    public Bullet SpawnBullet(Entity owner, Vector2 position, Vector2 direction)
    {
        Bullet bullet = new Bullet(owner);
        bullet.AddComponent(new ModelComponent(new List<Vector2> { Vector2.Zero }, new Vector3(1f, 1f, 1f)));
        bullet.AddComponent(new MovementComponent(0.0f, direction)); // TODO: velocity
        bullet.AddComponent(new PositionComponent(position, 0.0f));

        _entityController.AddEntity(bullet);

        return bullet;
    }
}