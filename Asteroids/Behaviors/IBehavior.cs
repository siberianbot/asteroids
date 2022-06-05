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

    void Update(float delta);

    bool Persistent
    {
        get => false;
    }
}