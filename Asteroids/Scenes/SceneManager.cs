using Asteroids.Engine;
using Asteroids.Entities;

namespace Asteroids.Scenes;

public class SceneManager
{
    private readonly List<Scene> _scenes;

    public SceneManager(Spawner spawner, CameraController cameraController)
    {
        _scenes = new List<Scene>
        {
            new TestbedScene(spawner, cameraController),
            new AsteroidsDemoScene(spawner, cameraController)
        };
    }

    public Scene GetScene(string name)
    {
        return _scenes.Single(scene => scene.Name == name);
    }
}