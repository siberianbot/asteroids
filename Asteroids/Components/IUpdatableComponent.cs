namespace Asteroids.Components;

// TODO: is it really needed to allow components to update their state?
public interface IUpdatableComponent : IComponent
{
    void Update(float delta);
}