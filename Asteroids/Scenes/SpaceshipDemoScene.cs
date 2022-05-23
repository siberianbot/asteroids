using System.Numerics;
using Asteroids.Entities;

namespace Asteroids.Scenes;

public class SpaceshipDemoScene : Scene
{
    private readonly Spawner _spawner;

    public SpaceshipDemoScene(Spawner spawner)
    {
        _spawner = spawner;
    }

    public override string Name
    {
        get => Constants.SpaceshipDemo;
    }

    public override void Load()
    {
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                Vector2 position = new Vector2(-5.0f + x * 2.5f, -2.5f + y * 2.5f);

                _spawner.SpawnSpaceship(position, null);
            }
        }
    }
}