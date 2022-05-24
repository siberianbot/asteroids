using Asteroids.Entities;

namespace Asteroids.Components;

public class Component : IComponent
{
    private Entity? _owner;

    public Entity Owner
    {
        get
        {
            if (_owner == null)
            {
                throw new Exception("Component ownership is not defined");
            }

            return _owner;
        }

        set
        {
            if (_owner != null)
            {
                throw new Exception("Attempt to re-define component ownership");
            }

            _owner = value;
        }
    }
}