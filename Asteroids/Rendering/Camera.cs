using System.Numerics;
using Asteroids.Components;
using Asteroids.Entities;
using Silk.NET.Maths;

namespace Asteroids.Rendering;

public class Camera
{
    private static readonly Vector3 Forward = new Vector3(0f, 0f, -1f);
    private static readonly Vector3 Up = new Vector3(0f, 1f, 0f);

    private readonly Entity _entity;

    public Camera(Entity entity)
    {
        _entity = entity;
    }

    public Vector2D<int> Dimensions { get; set; }

    public Matrix4x4 ViewMatrix
    {
        get
        {
            Vector3 position = new Vector3(_entity.GetComponent<PositionComponent>()?.Position ?? Vector2.Zero, 2f);

            return Matrix4x4.CreateLookAt(position, position + Forward, Up);
        }
    }

    public Matrix4x4 ProjectionMatrix
    {
        get
        {
            const float near = 0.00001f;
            const float far = 5.00000f;
            const float fov = MathF.PI / 2;
            float aspect = (float)Dimensions.X / Dimensions.Y;

            return Matrix4x4.CreatePerspectiveFieldOfView(fov, aspect, near, far);
        }
    }
}