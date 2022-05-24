using Asteroids.Commands;
using Asteroids.Scenes;

namespace Asteroids.Engine;

public class SceneController
{
    private readonly SceneManager _sceneManager;
    private readonly EntityController _entityController;
    private readonly CameraController _cameraController;
    private readonly BehaviorController _behaviorController;
    private readonly CommandQueue _commandQueue;

    public SceneController(
        SceneManager sceneManager,
        EntityController entityController,
        CameraController cameraController,
        BehaviorController behaviorController,
        CommandQueue commandQueue)
    {
        _sceneManager = sceneManager;
        _entityController = entityController;
        _cameraController = cameraController;
        _behaviorController = behaviorController;
        _commandQueue = commandQueue;
    }

    public void ChangeScene(string sceneName)
    {
        Scene scene = _sceneManager.GetScene(sceneName);

        _commandQueue.Push(() =>
        {
            _behaviorController.ClearBehaviors();
            _entityController.Clear();
            _cameraController.Reset();

            scene.Load();
        });
    }
}