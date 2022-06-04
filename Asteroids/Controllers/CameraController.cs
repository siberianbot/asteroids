using Asteroids.Entities;

namespace Asteroids.Controllers;

public class CameraController : IController
{
    public Camera? CurrentCamera { get; set; }

    public void Reset()
    {
        CurrentCamera = null;
    }
}