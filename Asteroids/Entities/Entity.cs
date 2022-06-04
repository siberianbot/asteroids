using Asteroids.Components;
using Asteroids.Engine;

namespace Asteroids.Entities;

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
        component.Owner = this;

        _components.Add(component);
    }

    public virtual void Create()
    {
        //
    }

    public virtual void Update(UpdateContext context)
    {
        foreach (IUpdatableComponent component in _components.OfType<IUpdatableComponent>())
        {
            component.Update(context);
        }
    }

    public virtual void Destroy()
    {
        //
    }
}