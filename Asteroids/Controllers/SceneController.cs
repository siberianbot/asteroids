using Asteroids.Engine;
using Asteroids.Scenes;

namespace Asteroids.Controllers;

public class SceneController : IController
{
    private readonly SceneManager _sceneManager;
    private readonly CommandQueue _commandQueue;
    private readonly EventQueue _eventQueue;
    private readonly Vars _vars;
    private long _sceneChangeSubscriptionIdx;

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

    public void Initialize()
    {
        _sceneChangeSubscriptionIdx = _eventQueue.Subscribe(EventType.SceneChange, @event =>
        {
            Scene scene = _sceneManager.GetScene(@event.SceneName!);

            _commandQueue.Push(() =>
            {
                scene.Load();
                _vars.SetVar(Constants.Vars.EngineTimeMultiplier, 1.0f); // TODO: remove
            });
        });
    }

    public void Terminate()
    {
        _eventQueue.Unsubscribe(EventType.SceneChange, _sceneChangeSubscriptionIdx);
    }
}