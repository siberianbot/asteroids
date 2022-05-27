using System.Numerics;
using Asteroids.Components;
using Asteroids.Entities;
using Asteroids.Physics;
using Asteroids.Utils;

namespace Asteroids.Behaviors;

// TODO: collision handling should be improved
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

    public void HandleCollision(Collision collision)
    {
        MovementComponent leftMovement = collision.Left.GetComponent<MovementComponent>() ?? throw new ArgumentException();
        PositionComponent leftPosition = collision.Left.GetComponent<PositionComponent>() ?? throw new ArgumentException();
        MovementComponent rightMovement = collision.Right.GetComponent<MovementComponent>() ?? throw new ArgumentException();
        PositionComponent rightPosition = collision.Right.GetComponent<PositionComponent>() ?? throw new ArgumentException();

        Vector2 sum = Vector2.Normalize(leftMovement.Direction + rightMovement.Direction);
        Vector2 sub = Vector2.Normalize(leftMovement.Direction - rightMovement.Direction);
        float velocity = (leftMovement.Velocity + rightMovement.Velocity) / 2;
        Vector2 intersection = MathUtils.GetIntersection(
                                   leftPosition.Position, leftPosition.Position + leftMovement.Direction,
                                   rightPosition.Position, rightPosition.Position + rightMovement.Direction)
                               ?? throw new ArgumentException();

        if (collision.Left is Asteroid leftAsteroid && leftAsteroid.Scale >= 0.5f)
        {
            float scale = leftAsteroid.Scale / 2;
            _spawner.SpawnAsteroid(intersection + sum * (scale + 0.05f), sum, velocity, scale);
            _spawner.SpawnAsteroid(intersection + sub * (scale + 0.05f), sub, velocity, scale);
        }

        if (collision.Right is Asteroid rightAsteroid && rightAsteroid.Scale >= 0.5f)
        {
            float scale = rightAsteroid.Scale / 2;
            _spawner.SpawnAsteroid(intersection - sum * (scale + 0.05f), -sum, velocity, scale);
            _spawner.SpawnAsteroid(intersection - sub * (scale + 0.05f), -sub, velocity, scale);
        }
    }
}