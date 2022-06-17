namespace Asteroids.Entities;

public class Camera : Entity, IOwnedEntity
{
    public Camera(Entity? owner)
    {
        Owner = owner;
    }

    public Entity? Owner { get; }
}