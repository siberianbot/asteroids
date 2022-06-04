using System.Drawing;
using System.Numerics;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace Asteroids.Rendering;

public class Renderer : IDisposable
{
    private const float Near = 0.00001f;
    private const float Far = 20.00000f;
    private const float Fov = MathF.PI / 2;

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
                                          "uniform vec3 color;\n" +
                                          "out vec4 FragColor;\n" +
                                          "void main()\n" +
                                          "{\n" +
                                          "  FragColor = vec4(color.x, color.y, color.z, 1.0);\n" +
                                          "}\n";

    private readonly GL _gl;
    private readonly BufferObject<float> _verticesBuffer;
    private readonly BufferObject<uint> _indicesBuffer;
    private readonly VertexArrayObject<float> _vertexArray;
    private readonly Shader _shader;
    private Matrix4x4 _projectionMatrix;

    public Renderer(GL gl)
    {
        _gl = gl;

        _verticesBuffer = new BufferObject<float>(_gl, BufferTargetARB.ArrayBuffer);
        _indicesBuffer = new BufferObject<uint>(_gl, BufferTargetARB.ElementArrayBuffer);
        _vertexArray = new VertexArrayObject<float>(_gl);

        _shader = new ShaderBuilder(_gl)
            .UseVertexShader(VertexShader)
            .UseFragmentShader(FragmentShader)
            .Build();
    }

    public void Clear()
    {
        _gl.ClearColor(Color.FromArgb(255, 0, 0, 0));
        _gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }

    public void UpdateDimensions(Vector2D<int> dimensions)
    {
        _gl.Viewport(dimensions);
        _projectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(Fov, (float)dimensions.X / dimensions.Y, Near, Far);
    }

    public unsafe void Render(RenderData renderData)
    {
        _shader.UseProgram();
        _shader.SetMat4("projection", _projectionMatrix);
        _shader.SetMat4("view", renderData.ViewMatrix);

        _vertexArray.Bind();

        foreach (ModelData modelData in renderData.Models)
        {
            _shader.SetMat4("transform", modelData.TransformMatrix);
            _shader.SetVec3("color", modelData.Color);

            _verticesBuffer.Data(modelData.VerticesData, BufferUsageARB.DynamicDraw);
            _indicesBuffer.Data(modelData.IndicesData, BufferUsageARB.DynamicDraw);
            _vertexArray.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2, 0);

            _gl.DrawElements(PrimitiveType.LineLoop, modelData.Count, DrawElementsType.UnsignedInt, null);
        }

        _gl.BindVertexArray(0);
    }

    #region IDisposable

    private bool _disposed;

    ~Renderer()
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