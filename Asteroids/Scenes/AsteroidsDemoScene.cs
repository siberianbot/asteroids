using System.Numerics;
using Asteroids.Engine;
using Asteroids.Entities;
using Asteroids.Rendering;

namespace Asteroids.Scenes;

public class AsteroidsDemoScene : Scene
{
    private readonly Spawner _spawner;
    private readonly CameraController _cameraController;

    public AsteroidsDemoScene(Spawner spawner, CameraController cameraController)
    {
        _spawner = spawner;
        _cameraController = cameraController;
    }

    public override string Name
    {
        get => Constants.AsteroidsDemo;
    }

    public override void Load()
    {
        _cameraController.CurrentCamera = new Camera(Vector2.Zero);

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