using System.Numerics;
using Asteroids.Rendering;

namespace Asteroids.Entities;

public interface IEntity
{
    void Update(UpdateContext context);

    void Render(RenderContext context);
    
    Vector2 Position { get; }
}

public class UpdateContext
{
    public float Delta { get; init; }
}

public class RenderContext
{
    public AsteroidRenderer AsteroidRenderer { get; init; }
}