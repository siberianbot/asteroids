using Asteroids.Engine;
using Asteroids.Entities;

namespace Asteroids.Components;

public interface IUpdatableComponent : IComponent
{
    void Update(UpdateContext context);
}