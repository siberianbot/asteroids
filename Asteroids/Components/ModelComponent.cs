using System.Numerics;
using Asteroids.Utils;

namespace Asteroids.Components;

public class ModelComponent : Component
{
    public ModelComponent(float[] verticesData, Vector3 color)
    {
        int count = verticesData.Length / 2;

        VerticesData = verticesData;
        IndicesData = GenerationUtils.GenerateIndicesData(count).ToArray();
        Count = (uint)count;
        Color = color;
    }

    public float[] VerticesData { get; }

    public uint[] IndicesData { get; }

    public uint Count { get; }

    public Vector3 Color { get; }
}