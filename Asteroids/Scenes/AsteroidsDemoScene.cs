using System.Numerics;
using Asteroids.Entities;

namespace Asteroids.Scenes;

public class AsteroidsDemoScene : Scene
{
    private readonly Spawner _spawner;

    public AsteroidsDemoScene(Spawner spawner)
    {
        _spawner = spawner;
    }

    public override string Name
    {
        get => Constants.Scenes.AsteroidsDemo;
    }

    public override void Load()
    {
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                Vector2 position = new Vector2(-5.0f + x * 2.5f, -2.5f + y * 2.5f);

                _spawner.SpawnAsteroid(position, Vector2.Zero);
            }
        }
    }
}