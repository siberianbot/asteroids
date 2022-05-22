using System.Numerics;

namespace Asteroids.Entities;

public class AsteroidFactory
{
    public Asteroid Create(Vector2 position, Vector2 direction)
    {
        float velocity = Random.Shared.NextSingle();
        float rotationVelocity = Random.Shared.NextSingle();

        List<Vector2> points = new List<Vector2>();
        Vector2 center = Vector2.Zero;

        for (int i = 0; i < Constants.AsteroidSpikesCount; i++)
        {
            float angle = MathF.Tau * i / Constants.AsteroidSpikesCount;
            float radius = 0.5f + Random.Shared.NextSingle() / 2;

            Vector2 point = new Vector2(
                radius * MathF.Cos(angle),
                radius * MathF.Sin(angle)
            );

            center += point;

            points.Add(point);
        }

        for (int i = 0; i < Constants.AsteroidSpikesCount; i++)
        {
            points[i] -= center;
        }

        return new Asteroid(position, velocity * direction, rotationVelocity, points);
    }
}