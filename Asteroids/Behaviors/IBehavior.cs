using Asteroids.Engine;

namespace Asteroids.Behaviors;

public interface IBehavior
{
    void Initialize()
    {
        //
    }

    void Terminate()
    {
        //
    }

    void Update(UpdateContext context);

    bool Persistent
    {
        get => false;
    }
}