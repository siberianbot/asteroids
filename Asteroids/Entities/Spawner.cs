using System.Numerics;
using Asteroids.Utils;

namespace Asteroids.Entities;

public class Spawner
{
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

        return new Asteroid(position, velocity * direction, rotationVelocity, points);
    }

    public Spaceship SpawnSpaceship(Vector2 position)
    {
        return new Spaceship(position);
    }

    public Bullet SpawnBullet(Entity owner, Vector2 position, Vector2 direction)
    {
        return new Bullet(owner, position, direction);
    }
}