using Asteroids.Engine;
using Silk.NET.Input;

namespace Asteroids.Controllers;

public class InputController : IController
{
    private readonly EventQueue _eventQueue;
    private readonly Dictionary<Key, bool> _keyStates = new Dictionary<Key, bool>();

    public InputController(IInputContext inputContext, EventQueue eventQueue)
    {
        _eventQueue = eventQueue;

        foreach (IKeyboard keyboard in inputContext.Keyboards)
        {
            keyboard.KeyDown += KeyPress;
            keyboard.KeyUp += KeyRelease;
        }
    }

    public bool IsKeyPressed(Key key)
    {
        return _keyStates.ContainsKey(key) && _keyStates[key];
    }

    private void KeyPress(IKeyboard keyboard, Key key, int _)
    {
        _keyStates[key] = true;
        _eventQueue.Push(new Event { EventType = EventType.KeyPress, Key = key });
    }

    private void KeyRelease(IKeyboard keyboard, Key key, int _)
    {
        _keyStates[key] = false;
        _eventQueue.Push(new Event { EventType = EventType.KeyRelease, Key = key });
    }
}