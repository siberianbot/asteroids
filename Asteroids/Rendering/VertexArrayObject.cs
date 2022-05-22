using Silk.NET.OpenGL;

namespace Asteroids.Rendering;

public class VertexArrayObject<TVertex> : IDisposable
    where TVertex : unmanaged
{
    private readonly GL _gl;
    private readonly uint _handle;

    public VertexArrayObject(GL gl)
    {
        _gl = gl;
        _handle = _gl.GenVertexArray();
    }

    public void Bind()
    {
        _gl.BindVertexArray(_handle);
    }

    public unsafe void VertexAttribPointer(uint index, int size, VertexAttribPointerType type, bool normalized, int stride, int offset)
    {
        _gl.VertexAttribPointer(index, size, type, normalized, (uint)(stride * sizeof(TVertex)), (void*)(offset * sizeof(TVertex)));
        _gl.EnableVertexArrayAttrib(_handle, index);
    }

    #region IDisposable

    private bool _disposed;

    ~VertexArrayObject()
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

        _gl.DeleteVertexArray(_handle);

        _disposed = true;
    }

    #endregion
}