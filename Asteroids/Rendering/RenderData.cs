using System.Numerics;

namespace Asteroids.Rendering;

public struct RenderData
{
    public RenderData(IEnumerable<ModelData> models, Matrix4x4 viewMatrix)
    {
        Models = models;
        ViewMatrix = viewMatrix;
    }

    public IEnumerable<ModelData> Models { get; }

    public Matrix4x4 ViewMatrix { get; }
}