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

        // TODO:
        // Unhandled exception. Silk.NET.Core.Loader.SymbolLoadingException: Native symbol not found (Symbol: glDeleteBuffers)
        //     at Silk.NET.GLFW.GlfwContext.<GetProcAddress>g__Throw|3_0(String proc)
        //     at Silk.NET.GLFW.GlfwContext.GetProcAddress(String proc, Nullable`1 slot)
        //     at Silk.NET.OpenGL.GL._B.get__RI()
        //     at Silk.NET.OpenGL.GL.DeleteBuffers(UInt32 n, UInt32* buffers)
        //     at Silk.NET.OpenGL.GL.DeleteBuffer(UInt32 buffers)
        //     at Asteroids.Rendering.BufferObject`1.InternalDispose() in /home/siberianbot/Code/Asteroids/Asteroids/Rendering/BufferObject.cs:line 53
        //     at Asteroids.Rendering.BufferObject`1.Dispose() in /home/siberianbot/Code/Asteroids/Asteroids/Rendering/BufferObject.cs:line 42
        //     at Asteroids.Rendering.AsteroidRenderer.InternalDispose() in /home/siberianbot/Code/Asteroids/Asteroids/Rendering/AsteroidRenderer.cs:line 97
        //     at Asteroids.Rendering.AsteroidRenderer.Dispose() in /home/siberianbot/Code/Asteroids/Asteroids/Rendering/AsteroidRenderer.cs:line 85
        //     at Asteroids.Engine.Engine.InternalDispose() in /home/siberianbot/Code/Asteroids/Asteroids/Engine/Engine.cs:line 115
        //     at Asteroids.Engine.Engine.Dispose() in /home/siberianbot/Code/Asteroids/Asteroids/Engine/Engine.cs:line 104
        //     at Asteroids.Program.Main() in /home/siberianbot/Code/Asteroids/Asteroids/Program.cs:line 10
        _gl.DeleteBuffer(_handle);

        _disposed = true;
    }

    #endregion
}