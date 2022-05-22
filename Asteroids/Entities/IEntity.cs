using Asteroids.Rendering;

namespace Asteroids.Entities;

public interface IEntity
{
    void Update(UpdateContext context);

    void Render(RenderContext context);
}

public class UpdateContext
{
    public float Delta { get; init; }
}

public class RenderContext
{
    public AsteroidRenderer AsteroidRenderer { get; init; }
}