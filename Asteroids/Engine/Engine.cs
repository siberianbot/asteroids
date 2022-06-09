namespace Asteroids.Engine;

public sealed class Engine : IDisposable
{
    private readonly Server.Server _server;
    private readonly Client.Client _client;
    private bool _disposed;

    public Engine()
    {
        _server = new Server.Server();
        _client = new Client.Client(_server);
    }

    public void Run()
    {
        _server.Start();
        _client.Run();
    }

    #region IDisposable

    ~Engine()
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

        _client.Dispose();
        _server.Stop();

        _disposed = true;
    }

    #endregion
}