namespace Asteroids.Controllers;

public class ControllersCollection
{
    private readonly Dictionary<Type, IController> _controllers;

    public ControllersCollection()
    {
        _controllers = new Dictionary<Type, IController>();
    }

    public void AddController<TController>(TController controller)
        where TController : class, IController
    {
        Type type = typeof(TController);

        if (_controllers.ContainsKey(type))
        {
            throw new Exception($"controller {type.Name} already presented");
        }

        _controllers.Add(type, controller);
    }

    public TController GetController<TController>()
        where TController : class, IController
    {
        Type type = typeof(TController);

        if (!_controllers.ContainsKey(type))
        {
            throw new Exception($"controller {type.Name} is not presented");
        }

        return (TController)_controllers[type];
    }

    public void InitializeAll()
    {
        foreach ((_, IController controller) in _controllers)
        {
            controller.Initialize();
        }
    }

    public void TerminateAll()
    {
        foreach ((_, IController controller) in _controllers)
        {
            controller.Terminate();
        }
    }
}