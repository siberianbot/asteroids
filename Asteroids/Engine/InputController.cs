using Silk.NET.Input;

namespace Asteroids.Engine;

public class InputController
{
    private readonly IInputContext _inputContext;

    public InputController(IInputContext inputContext)
    {
        _inputContext = inputContext;
    }

    public bool OnKeyPressed(Key key)
    {
        return _inputContext.Keyboards.Any(keyboard => keyboard.IsKeyPressed(key));
    }
}