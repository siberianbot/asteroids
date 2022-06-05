using Asteroids.Behaviors;
using Asteroids.Engine;

namespace Asteroids.Controllers;

public class BehaviorController : IController
{
    private readonly CommandQueue _commandQueue;
    private readonly EventQueue _eventQueue;
    private readonly List<IBehavior> _behaviors = new List<IBehavior>();
    private long _sceneChangeSubscription;

    public BehaviorController(CommandQueue commandQueue, EventQueue eventQueue)
    {
        _commandQueue = commandQueue;
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

    public IReadOnlyCollection<IBehavior> Behaviors
    {
        get => _behaviors;
    }

    public void AddBehavior<TBehavior>(TBehavior behavior)
        where TBehavior : IBehavior
    {
        if (_behaviors.Any(b => b is TBehavior))
        {
            throw new Exception($"behavior {typeof(TBehavior).Name} already presented");
        }

        _commandQueue.Push(() =>
        {
            _behaviors.Add(behavior);
            behavior.Initialize();
        });
    }

    public void RemoveBehavior<TBehavior>()
        where TBehavior : IBehavior
    {
        TBehavior? behavior = _behaviors.OfType<TBehavior>().SingleOrDefault();

        if (behavior == null)
        {
            return;
        }

        _commandQueue.Push(() =>
        {
            behavior.Terminate();
            _behaviors.Remove(behavior);
        });
    }

    public void Reset()
    {
        IBehavior[] behaviors = _behaviors
            .Where(behavior => !behavior.Persistent)
            .ToArray();

        foreach (IBehavior behavior in behaviors)
        {
            behavior.Terminate();

            _behaviors.Remove(behavior);
        }
    }
}