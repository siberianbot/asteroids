using Asteroids.Behaviors;
using Asteroids.Engine;

namespace Asteroids.Controllers;

public class BehaviorController : IController
{
    private readonly CommandQueue _commandQueue;
    private readonly List<IBehavior> _behaviors = new List<IBehavior>();

    public BehaviorController(CommandQueue commandQueue)
    {
        _commandQueue = commandQueue;
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

        _commandQueue.Push(() => _behaviors.Add(behavior));
    }

    public void Reset()
    {
        _behaviors.RemoveAll(behavior => !behavior.Persistent);
    }
}