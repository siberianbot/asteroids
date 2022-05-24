using System.Numerics;

namespace Asteroids.Rendering;

public struct RenderData
{
    public RenderData(float[] verticesData, uint[] indicesData, uint count, Vector3 color, Matrix4x4 transformMatrix)
    {
        VerticesData = verticesData;
        IndicesData = indicesData;
        Count = count;
        Color = color;
        TransformMatrix = transformMatrix;
    }

    public float[] VerticesData { get; }

    public uint[] IndicesData { get; }

    public uint Count { get; }

    public Vector3 Color { get; }

    public Matrix4x4 TransformMatrix { get; }
}