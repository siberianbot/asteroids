using Silk.NET.OpenGL;

namespace Asteroids.Rendering;

public class BufferObject<T> : IDisposable
    where T : unmanaged
{
    private readonly GL _gl;
    private readonly BufferTargetARB _type;
    private readonly uint _handle;

    public BufferObject(GL gl, BufferTargetARB type)
    {
        _gl = gl;
        _type = type;

        _handle = _gl.CreateBuffer();
    }

    public void Data(ReadOnlySpan<T> data, BufferUsageARB usage)
    {
        Bind();
        _gl.BufferData(_type, data, usage);
    }

    public void Bind()
    {
        _gl.BindBuffer(_type, _handle);
    }

    #region IDisposable

    private bool _disposed;

    ~BufferObject()
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

        _gl.DeleteBuffer(_handle);

        _disposed = true;
    }

    #endregion
}