using Silk.NET.Input;

namespace Asteroids.Input;

public class InputProcessor
{
    private enum KeyState
    {
        Press,
        Release
    }

    private struct KeyEvent
    {
        public KeyEvent(Key key, KeyState state)
        {
            Key = key;
            State = state;
        }

        public Key Key { get; }

        public KeyState State { get; }
    }

    private readonly Queue<KeyEvent> _pending = new Queue<KeyEvent>();
    private readonly Dictionary<Key, Action> _keyPress = new Dictionary<Key, Action>();
    private readonly Dictionary<Key, Action> _keyRelease = new Dictionary<Key, Action>();

    public bool Enabled { get; set; }

    public void PushKeyPress(Key key)
    {
        _pending.Enqueue(new KeyEvent(key, KeyState.Press));
    }

    public void PushKeyRelease(Key key)
    {
        _pending.Enqueue(new KeyEvent(key, KeyState.Release));
    }

    public void OnKeyPress(Key key, Action action)
    {
        _keyPress[key] = action;
    }

    public void OnKeyRelease(Key key, Action action)
    {
        _keyRelease[key] = action;
    }

    public void ExecutePending()
    {
        if (Enabled)
        {
            foreach (KeyEvent @event in _pending)
            {
                switch (@event.State)
                {
                    case KeyState.Press when _keyPress.TryGetValue(@event.Key, out Action? keyPressAction):
                        keyPressAction.Invoke();
                        break;

                    case KeyState.Release when _keyRelease.TryGetValue(@event.Key, out Action? keyReleaseAction):
                        keyReleaseAction.Invoke();
                        break;
                }
            }
        }

        _pending.Clear();
    }
}