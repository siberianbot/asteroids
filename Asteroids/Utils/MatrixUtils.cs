using System.Numerics;

namespace Asteroids.Utils;

public static class MatrixUtils
{
    private static readonly Vector3 Forward = new Vector3(0f, 0f, -1f);
    private static readonly Vector3 Up = new Vector3(0f, 1f, 0f);

    public static Matrix4x4 GetViewMatrix(Vector2 position)
    {
        // TODO: hardcoded Z = 5.0f 
        return Matrix4x4.CreateLookAt(new Vector3(position, 5.0f), new Vector3(position, 5.0f) + Forward, Up);
    }
}