using System.Numerics;
using Asteroids.Entities;

namespace Asteroids.Engine;

public class CameraController
{
    private readonly Spawner _spawner;
    private Camera _defaultCamera;
    private Camera? _currentCamera;

    public CameraController(Spawner spawner)
    {
        _spawner = spawner;
        _defaultCamera = spawner.SpawnCamera(Vector2.Zero);
    }

    public Camera CurrentCamera
    {
        get => _currentCamera ?? _defaultCamera;
        set => _currentCamera = value;
    }

    public void Reset()
    {
        _currentCamera = null;
        _defaultCamera = _spawner.SpawnCamera(Vector2.Zero);
    }
}