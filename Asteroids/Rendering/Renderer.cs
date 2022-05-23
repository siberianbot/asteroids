using System.Drawing;
using Asteroids.Components;
using Asteroids.Engine;
using Asteroids.Entities;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace Asteroids.Rendering;

public class Renderer : IDisposable
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
                                          "uniform vec3 color;\n" +
                                          "out vec4 FragColor;\n" +
                                          "void main()\n" +
                                          "{\n" +
                                          "  FragColor = vec4(color.x, color.y, color.z, 1.0);\n" +
                                          "}\n";

    private readonly GL _gl;
    private readonly CameraController _cameraController;

    private readonly BufferObject<float> _verticesBuffer;
    private readonly BufferObject<uint> _indicesBuffer;
    private readonly VertexArrayObject<float> _vertexArray;
    private readonly Shader _shader;

    public Renderer(GL gl, CameraController cameraController)
    {
        _gl = gl;
        _cameraController = cameraController;

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
    }

    public unsafe void Render(Entity entity)
    {
        ModelComponent modelComponent = entity.GetComponent<ModelComponent>() ?? throw new NullReferenceException();
        PositionComponent positionComponent = entity.GetComponent<PositionComponent>() ?? throw new NullReferenceException();

        _shader.UseProgram();
        _shader.SetMat4("transform", positionComponent.TransformMatrix);
        _shader.SetMat4("projection", _cameraController.CurrentCamera.ProjectionMatrix);
        _shader.SetMat4("view", _cameraController.CurrentCamera.ViewMatrix);
        _shader.SetVec3("color", modelComponent.Color);

        _vertexArray.Bind();

        _verticesBuffer.Data(modelComponent.VerticesData, BufferUsageARB.DynamicDraw);
        _indicesBuffer.Data(modelComponent.IndicesData, BufferUsageARB.DynamicDraw);

        _vertexArray.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2, 0);

        _gl.DrawElements(PrimitiveType.LineLoop, modelComponent.Count, DrawElementsType.UnsignedInt, null);
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