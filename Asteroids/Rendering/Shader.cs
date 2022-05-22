using System.Numerics;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Boolean = Silk.NET.OpenGL.Boolean;

namespace Asteroids.Rendering;

public class Shader : IDisposable
{
    private readonly GL _gl;
    private readonly uint _handle;

    public Shader(GL gl, uint handle)
    {
        _gl = gl;
        _handle = handle;
    }

    public void UseProgram()
    {
        _gl.UseProgram(_handle);
    }

    public void SetVec3(string variable, Vector3 value)
    {
        int location = _gl.GetUniformLocation(_handle, variable);
        _gl.Uniform3(location, value);
    }

    public unsafe void SetMat4(string variable, Matrix4x4 value)
    {
        int location = _gl.GetUniformLocation(_handle, variable);
        _gl.UniformMatrix4(location, 1, Boolean.False, (float*)&value);
    }

    #region IDisposable

    private bool _disposed;

    ~Shader()
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

        _gl.DeleteProgram(_handle);

        _disposed = true;
    }

    #endregion
}