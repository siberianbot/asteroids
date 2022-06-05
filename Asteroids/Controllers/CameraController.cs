using Asteroids.Engine;
using Asteroids.Entities;

namespace Asteroids.Controllers;

public class CameraController : IController
{
    private readonly EventQueue _eventQueue;
    private long _sceneChangeSubscription;

    public CameraController(EventQueue eventQueue)
    {
        _eventQueue = eventQueue;
    }

    public void Initialize()
    {
        _sceneChangeSubscription = _eventQueue.Subscribe(EventType.SceneChange, _ => Reset());
    }

    public void Terminate()
    {
        _eventQueue.Unsubscribe(EventType.SceneChange, _sceneChangeSubscription);
    }

    public Camera? CurrentCamera { get; set; }

    public void Reset()
    {
        CurrentCamera = null;
    }
}