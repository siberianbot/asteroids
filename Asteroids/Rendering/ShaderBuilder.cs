using Silk.NET.OpenGL;

namespace Asteroids.Rendering;

public class ShaderBuilder
{
    private readonly GL _gl;
    private readonly List<uint> _shaders = new List<uint>();

    public ShaderBuilder(GL gl)
    {
        _gl = gl;
    }

    public ShaderBuilder UseVertexShader(string content)
    {
        uint handle = _gl.CreateShader(ShaderType.VertexShader);

        _gl.ShaderSource(handle, content);
        _gl.CompileShader(handle);

        _shaders.Add(handle);

        return this;
    }

    public ShaderBuilder UseFragmentShader(string content)
    {
        uint handle = _gl.CreateShader(ShaderType.FragmentShader);

        _gl.ShaderSource(handle, content);
        _gl.CompileShader(handle);

        _shaders.Add(handle);

        return this;
    }

    public Shader Build()
    {
        uint handle = _gl.CreateProgram();

        foreach (uint shader in _shaders)
        {
            _gl.AttachShader(handle, shader);
        }

        _gl.LinkProgram(handle);

        foreach (uint shader in _shaders)
        {
            _gl.DeleteShader(shader);
        }

        return new Shader(_gl, handle);
    }
}