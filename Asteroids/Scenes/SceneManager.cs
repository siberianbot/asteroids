using Asteroids.Behaviors;
using Asteroids.Controllers;
using Asteroids.Entities;

namespace Asteroids.Scenes;

public class SceneManager
{
    private readonly List<Scene> _scenes;

    public SceneManager(Spawner spawner, BehaviorFactory behaviorFactory, ControllersCollection controllersCollection)
    {
        _scenes = new List<Scene>
        {
            new TestbedScene(spawner, behaviorFactory, controllersCollection),
            new AsteroidsDemoScene(spawner),
            new SpaceshipDemoScene(spawner),
            new AsteroidCollisionScene(spawner, behaviorFactory, controllersCollection),
            new PlayableDemoScene(spawner, behaviorFactory, controllersCollection)
        };
    }

    public Scene GetScene(string name)
    {
        return _scenes.Single(scene => scene.Name == name);
    }
}