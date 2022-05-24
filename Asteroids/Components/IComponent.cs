using Asteroids.Entities;

namespace Asteroids.Components;

public interface IComponent
{
    Entity Owner { get; set; }
}