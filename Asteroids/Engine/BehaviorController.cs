using Asteroids.Behaviors;

namespace Asteroids.Engine;

public class BehaviorController
{
    private readonly List<IBehavior> _behaviors = new List<IBehavior>();

    public void AddBehavior(IBehavior behavior)
    {
        _behaviors.Add(behavior);
    }

    public void ForEachBehavior(Action<IBehavior> action)
    {
        foreach (IBehavior behavior in _behaviors)
        {
            action(behavior);
        }
    }

    public void ClearBehaviors()
    {
        _behaviors.RemoveAll(behavior => !behavior.Persistent);
    }
}