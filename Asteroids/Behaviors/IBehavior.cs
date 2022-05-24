using Asteroids.Entities;

namespace Asteroids.Behaviors;

public interface IBehavior
{
    void Update(UpdateContext context);

    bool Persistent
    {
        get => false;
    }
}