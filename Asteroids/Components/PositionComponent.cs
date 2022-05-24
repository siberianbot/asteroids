using System.Numerics;

namespace Asteroids.Components;

public class PositionComponent : Component
{
    public PositionComponent(Vector2 position, float rotation)
    {
        Position = position;
        Rotation = rotation;
    }

    public Vector2 Position { get; set; }

    public float Rotation { get; set; }

    public Matrix4x4 TransformMatrix
    {
        get => Matrix4x4.CreateRotationZ(Rotation) *
               Matrix4x4.CreateTranslation(new Vector3(Position, -1.0f)) *
               Matrix4x4.Identity;
    }
}