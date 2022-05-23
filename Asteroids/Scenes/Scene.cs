namespace Asteroids.Scenes;

public abstract class Scene
{
    public abstract string Name { get; }

    public abstract void Load();
}