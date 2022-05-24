using System.Numerics;
using Asteroids.Components;
using Asteroids.Entities;

namespace Asteroids.Behaviors;

public class AsteroidSpawnBehavior : IBehavior
{
    private readonly Spawner _spawner;

    public AsteroidSpawnBehavior(Spawner spawner)
    {
        _spawner = spawner;
    }

    public void Update(UpdateContext context)
    {
        // throw new NotImplementedException();
    }

    public void HandleCollision(Entity left, Entity right, Vector2 intersection)
    {
        MovementComponent leftMovement = left.GetComponent<MovementComponent>() ?? throw new ArgumentException();
        MovementComponent rightMovement = right.GetComponent<MovementComponent>() ?? throw new ArgumentException();

        Vector2 sum = Vector2.Normalize(leftMovement.Direction + rightMovement.Direction);
        Vector2 sub = Vector2.Normalize(leftMovement.Direction - rightMovement.Direction);
        float velocity = (leftMovement.Velocity + rightMovement.Velocity) / 2;

        if (left is Asteroid leftAsteroid && leftAsteroid.Scale >= 0.5f)
        {
            float scale = leftAsteroid.Scale / 2;
            _spawner.SpawnAsteroid(intersection + sum * (scale + 0.05f), sum, velocity, scale);
            _spawner.SpawnAsteroid(intersection + sub * (scale + 0.05f), sub, velocity, scale);
        }

        if (right is Asteroid rightAsteroid && rightAsteroid.Scale >= 0.5f)
        {
            float scale = rightAsteroid.Scale / 2;
            _spawner.SpawnAsteroid(intersection - sum * (scale + 0.05f), -sum, velocity, scale);
            _spawner.SpawnAsteroid(intersection - sub * (scale + 0.05f), -sub, velocity, scale);
        }
    }
}