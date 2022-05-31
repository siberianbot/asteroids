namespace Asteroids.Entities;

public interface IOwnedEntity
{
    Entity? Owner { get; } 
}