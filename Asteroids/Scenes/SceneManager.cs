using Asteroids.Controllers;
using Asteroids.Engine;
using Asteroids.Entities;

namespace Asteroids.Scenes;

public class SceneManager
{
    private readonly List<Scene> _scenes;

    public SceneManager(Spawner spawner, CameraController cameraController, BehaviorController behaviorController, Vars vars)
    {
        _scenes = new List<Scene>
        {
            new TestbedScene(spawner, cameraController, behaviorController, vars),
            new AsteroidsDemoScene(spawner, vars),
            new SpaceshipDemoScene(spawner, vars),
            new AsteroidCollisionScene(spawner, behaviorController, vars),
            new PlayableDemoScene(spawner, vars, cameraController, behaviorController)
        };
    }

    public Scene GetScene(string name)
    {
        return _scenes.Single(scene => scene.Name == name);
    }
}