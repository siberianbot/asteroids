using Asteroids.Components;

namespace Asteroids.Entities;

public class UpdateContext
{
    public float Delta { get; init; }
}

public abstract class Entity
{
    private readonly List<IComponent> _components = new List<IComponent>();

    public TComponent? GetComponent<TComponent>()
        where TComponent : class, IComponent
    {
        foreach (IComponent component in _components)
        {
            if (component is TComponent concreteComponent)
            {
                return concreteComponent;
            }
        }

        return null;
    }

    public void AddComponent(IComponent component)
    {
        _components.Add(component);
    }

    public abstract void Update(UpdateContext context);
}