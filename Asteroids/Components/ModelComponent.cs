using System.Numerics;
using Asteroids.Utils;

namespace Asteroids.Components;

public class ModelComponent : IComponent
{
    public ModelComponent(List<Vector2> points, Vector3 color)
    {
        VerticesData = points.SelectMany(point => new[] { point.X, point.Y }).ToArray();
        IndicesData = GenerationUtils.GenerateIndicesData(points.Count).ToArray();
        Count = (uint)points.Count;
        Color = color;
    }

    public float[] VerticesData { get; }

    public uint[] IndicesData { get; }

    public uint Count { get; }

    public Vector3 Color { get; }
}