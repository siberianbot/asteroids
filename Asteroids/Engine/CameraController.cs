using System.Numerics;
using Asteroids.Rendering;
using Silk.NET.Maths;

namespace Asteroids.Engine;

public class CameraController
{
    private readonly Camera _defaultCamera = new Camera(Vector2.Zero);
    private Camera? _currentCamera;

    public Camera CurrentCamera
    {
        get => _currentCamera ?? _defaultCamera;
        set
        {
            _currentCamera = value;
            
            // TODO: workaround
            _currentCamera.Dimensions = _defaultCamera.Dimensions;
        }
    }

    public void UpdateDimensions(Vector2D<int> dimensions)
    {
        if (_currentCamera != null)
        {
            _currentCamera.Dimensions = dimensions;
        }

        _defaultCamera.Dimensions = dimensions;
    }
}