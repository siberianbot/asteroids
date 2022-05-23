using Asteroids.Scenes;

namespace Asteroids.Engine;

public class SceneController
{
    private readonly SceneManager _sceneManager;
    private readonly EntityController _entityController;
    private readonly CameraController _cameraController;

    public SceneController(SceneManager sceneManager, EntityController entityController, CameraController cameraController)
    {
        _sceneManager = sceneManager;
        _entityController = entityController;
        _cameraController = cameraController;
    }

    public void ChangeScene(string sceneName)
    {
        Scene scene = _sceneManager.GetScene(sceneName);

        _entityController.Clear();
        _cameraController.Reset();

        scene.Load();
    }
}