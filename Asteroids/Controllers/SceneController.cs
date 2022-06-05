using Asteroids.Engine;
using Asteroids.Scenes;

namespace Asteroids.Controllers;

public class SceneController : IController
{
    private readonly SceneManager _sceneManager;
    private readonly CommandQueue _commandQueue;
    private readonly EventQueue _eventQueue;
    private readonly Vars _vars;

    public SceneController(
        SceneManager sceneManager,
        CommandQueue commandQueue,
        EventQueue eventQueue,
        Vars vars)
    {
        _sceneManager = sceneManager;
        _commandQueue = commandQueue;
        _eventQueue = eventQueue;
        _vars = vars;
    }

    public void ChangeScene(string sceneName)
    {
        Scene scene = _sceneManager.GetScene(sceneName);

        _eventQueue.Push(new Event
        {
            EventType = EventType.SceneChange
        });

        _commandQueue.Push(() =>
        {
            scene.Load();
            _vars.SetVar(Constants.Vars.EngineTimeMultiplier, 1.0f);
        });
    }
}