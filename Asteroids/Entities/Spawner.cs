using System.Numerics;
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

        Asteroid asteroid = new Asteroid(position, velocity * direction, rotationVelocity, points);

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

        Spaceship spaceship = new Spaceship(position, rotation, color.Value);

        _entityController.AddEntity(spaceship);

        return spaceship;
    }

    public Bullet SpawnBullet(Entity owner, Vector2 position, Vector2 direction)
    {
        Bullet bullet = new Bullet(owner, position, direction);

        _entityController.AddEntity(bullet);

        return bullet;
    }
}