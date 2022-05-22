using System.Numerics;
using Silk.NET.OpenGL;

namespace Asteroids.Rendering;

public class AsteroidRenderer : IDisposable
{
    private const string VertexShader = "#version 330 core\n" +
                                        "layout (location = 0) in vec2 pos;\n" +
                                        "uniform mat4 transform;\n" +
                                        "uniform mat4 projection;\n" +
                                        "uniform mat4 view;\n" +
                                        "void main()\n" +
                                        "{\n" +
                                        "  gl_Position = projection * view * transform * vec4(pos.x, pos.y, 0.0, 1.0);\n" +
                                        "}\n";

    private const string FragmentShader = "#version 330 core\n" +
                                          "out vec4 FragColor;\n" +
                                          "void main()\n" +
                                          "{\n" +
                                          "  FragColor = vec4(0.7, 0.7, 0.7, 1.0);\n" +
                                          "}\n";

    private readonly GL _gl;
    private readonly Camera _camera;

    private readonly BufferObject<float> _verticesBuffer;
    private readonly BufferObject<uint> _indicesBuffer;
    private readonly VertexArrayObject<float> _vertexArray;
    private readonly Shader _shader;

    public AsteroidRenderer(GL gl, Camera camera)
    {
        _gl = gl;
        _camera = camera;

        _verticesBuffer = new BufferObject<float>(_gl, BufferTargetARB.ArrayBuffer);
        _indicesBuffer = new BufferObject<uint>(_gl, BufferTargetARB.ElementArrayBuffer);
        _vertexArray = new VertexArrayObject<float>(_gl);


        uint[] indicesData = IndicesDataGenerator(Constants.AsteroidSpikesCount).ToArray();
        _indicesBuffer.Data(indicesData.AsSpan(), BufferUsageARB.StaticDraw);

        _shader = new ShaderBuilder(_gl)
            .UseVertexShader(VertexShader)
            .UseFragmentShader(FragmentShader)
            .Build();
    }

    public unsafe void Render(List<Vector2> points, Vector2 position, float rotation)
    {
        Span<float> data = points.SelectMany(point => new[] { point.X, point.Y }).ToArray().AsSpan();
        Matrix4x4 transform = Matrix4x4.CreateRotationZ(rotation) *
                              Matrix4x4.CreateTranslation(new Vector3(position, -1.0f)) *
                              Matrix4x4.Identity;

        _shader.UseProgram();
        _shader.SetMat4("transform", transform);
        _shader.SetMat4("projection", _camera.ProjectionMatrix);
        _shader.SetMat4("view", _camera.ViewMatrix);

        _vertexArray.Bind();
        _verticesBuffer.Data(data, BufferUsageARB.DynamicDraw);
        _indicesBuffer.Bind();
        _vertexArray.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2, 0);
        _gl.DrawElements(PrimitiveType.LineLoop, (uint)points.Count, DrawElementsType.UnsignedInt, null);
        _gl.BindVertexArray(0);
    }

    private static IEnumerable<uint> IndicesDataGenerator(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return (uint)i;
        }
    }

    #region IDisposable

    private bool _disposed;

    ~AsteroidRenderer()
    {
        InternalDispose();
    }

    public void Dispose()
    {
        InternalDispose();
        GC.SuppressFinalize(this);
    }

    private void InternalDispose()
    {
        if (_disposed)
        {
            return;
        }

        _vertexArray.Dispose();
        _verticesBuffer.Dispose();
        _indicesBuffer.Dispose();
        _shader.Dispose();

        _disposed = true;
    }

    #endregion
}