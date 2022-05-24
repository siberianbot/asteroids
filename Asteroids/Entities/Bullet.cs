namespace Asteroids.Entities;

public class Bullet : Entity
{
    public Bullet(Entity owner)
    {
        Owner = owner;
    }

    public Entity Owner { get; }
}