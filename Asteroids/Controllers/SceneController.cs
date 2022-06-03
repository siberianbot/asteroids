using Asteroids.Engine;
using Asteroids.Scenes;

namespace Asteroids.Controllers;

public class SceneController : IController
{
    private readonly SceneManager _sceneManager;
    private readonly EntityController _entityController;
    private readonly CameraController _cameraController;
    private readonly BehaviorController _behaviorController;
    private readonly PlayerController _playerController;
    private readonly CommandQueue _commandQueue;

    public SceneController(
        SceneManager sceneManager,
        EntityController entityController,
        CameraController cameraController,
        BehaviorController behaviorController,
        PlayerController playerController,
        CommandQueue commandQueue)
    {
        _sceneManager = sceneManager;
        _entityController = entityController;
        _cameraController = cameraController;
        _behaviorController = behaviorController;
        _playerController = playerController;
        _commandQueue = commandQueue;
    }

    public void ChangeScene(string sceneName)
    {
        Scene scene = _sceneManager.GetScene(sceneName);

        _commandQueue.Push(() =>
        {
            _behaviorController.Reset();
            _playerController.Reset();
            _entityController.Reset();
            _cameraController.Reset();

            scene.Load();
        });
    }
}