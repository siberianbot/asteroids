using Asteroids.Scenes;

namespace Asteroids.Engine;

public class SceneController
{
    private readonly SceneManager _sceneManager;
    private readonly EntityController _entityController;

    public SceneController(SceneManager sceneManager, EntityController entityController)
    {
        _sceneManager = sceneManager;
        _entityController = entityController;
    }

    public void ChangeScene(string sceneName)
    {
        Scene scene = _sceneManager.GetScene(sceneName);

        _entityController.Clear();

        scene.Load();
    }
}