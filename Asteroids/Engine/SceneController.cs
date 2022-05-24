using Asteroids.Scenes;

namespace Asteroids.Engine;

public class SceneController
{
    private readonly SceneManager _sceneManager;
    private readonly EntityController _entityController;
    private readonly CameraController _cameraController;
    private readonly BehaviorController _behaviorController;

    private Scene? _targetScene;

    public SceneController(SceneManager sceneManager, EntityController entityController, CameraController cameraController,
        BehaviorController behaviorController)
    {
        _sceneManager = sceneManager;
        _entityController = entityController;
        _cameraController = cameraController;
        _behaviorController = behaviorController;
    }

    public void ChangeScene(string sceneName)
    {
        _targetScene = _sceneManager.GetScene(sceneName);
    }

    public void PerformSceneChange()
    {
        if (_targetScene == null)
        {
            return;
        }

        _behaviorController.ClearBehaviors();
        _entityController.Clear();
        _cameraController.Reset();

        _targetScene.Load();
        _targetScene = null;
    }
}