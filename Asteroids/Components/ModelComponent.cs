using System.Numerics;
using Asteroids.Utils;

namespace Asteroids.Components;

public class ModelComponent : IComponent
{
    public ModelComponent(List<Vector2> points)
    {
        VerticesData = points.SelectMany(point => new[] { point.X, point.Y }).ToArray();
        IndicesData = GenerationUtils.GenerateIndicesData(points.Count).ToArray();
        Count = (uint)points.Count;
    }

    public float[] VerticesData { get; }

    public uint[] IndicesData { get; }

    public uint Count { get; }
}