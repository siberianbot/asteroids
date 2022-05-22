using Asteroids.Entities;

namespace Asteroids.Rendering;

public class Camera
{
    private readonly IEntity _entity;

    public Camera(IEntity entity)
    {
        _entity = entity;
    }
}