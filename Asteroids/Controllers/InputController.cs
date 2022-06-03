using Asteroids.Engine;
using Silk.NET.Input;

namespace Asteroids.Controllers;

public class InputController : IController
{
    public delegate void KeyPressCallback();

    public delegate void KeyReleaseCallback();

    private readonly CommandQueue _commandQueue;
    private readonly Dictionary<Key, bool> _keyStates = new Dictionary<Key, bool>();
    private readonly Dictionary<Key, KeyPressCallback> _pressCallbacks = new Dictionary<Key, KeyPressCallback>();
    private readonly Dictionary<Key, KeyReleaseCallback> _releaseCallbacks = new Dictionary<Key, KeyReleaseCallback>();

    public InputController(IInputContext inputContext, CommandQueue commandQueue)
    {
        _commandQueue = commandQueue;

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

    public void OnKeyPress(Key key, KeyPressCallback callback)
    {
        if (!_pressCallbacks.ContainsKey(key))
        {
            _pressCallbacks.Add(key, callback);
        }
        else
        {
            _pressCallbacks[key] = (KeyPressCallback)Delegate.Combine(_pressCallbacks[key], callback);
        }
    }

    public void OnKeyRelease(Key key, KeyReleaseCallback callback)
    {
        if (!_releaseCallbacks.ContainsKey(key))
        {
            _releaseCallbacks.Add(key, callback);
        }
        else
        {
            _releaseCallbacks[key] = (KeyReleaseCallback)Delegate.Combine(_releaseCallbacks[key], callback);
        }
    }

    public void Reset()
    {
        _pressCallbacks.Clear();
        _releaseCallbacks.Clear();
    }

    private void KeyPress(IKeyboard keyboard, Key key, int _)
    {
        _keyStates[key] = true;

        if (!_pressCallbacks.ContainsKey(key))
        {
            return;
        }

        _commandQueue.Push(() => _pressCallbacks[key].Invoke());
    }

    private void KeyRelease(IKeyboard keyboard, Key key, int _)
    {
        _keyStates[key] = false;

        if (!_releaseCallbacks.ContainsKey(key))
        {
            return;
        }

        _commandQueue.Push(() => _releaseCallbacks[key].Invoke());
    }
}