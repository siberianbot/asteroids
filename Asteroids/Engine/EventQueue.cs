namespace Asteroids.Engine;

public class EventQueue
{
    public delegate void HandlerDelegate(Event @event);

    private struct Handler
    {
        public long Index { get; init; }

        public HandlerDelegate Delegate { get; init; }
    }

    private readonly Dictionary<EventType, List<Handler>> _handlers = new Dictionary<EventType, List<Handler>>();
    private List<Event> _pendingEvents = new List<Event>();
    private readonly object _pendingEventsLock = new object();
    private long _idx;

    public void Push(Event @event)
    {
        lock (_pendingEventsLock)
        {
            _pendingEvents.Add(@event);
        }
    }

    public void ExecutePending()
    {
        List<Event> events;

        lock (_pendingEventsLock)
        {
            events = _pendingEvents;

            _pendingEvents = new List<Event>();
        }

        foreach (Event @event in events)
        {
            if (!_handlers.ContainsKey(@event.EventType))
            {
                continue;
            }

            foreach (Handler handler in _handlers[@event.EventType])
            {
                handler.Delegate(@event);
            }
        }
    }

    public void Reset()
    {
        lock (_pendingEventsLock)
        {
            _pendingEvents.Clear();
        }

        _handlers.Clear();
    }

    public long Subscribe(EventType expectedType, HandlerDelegate @delegate)
    {
        Handler handler = new Handler
        {
            Index = ++_idx,
            Delegate = @delegate
        };

        if (!_handlers.ContainsKey(expectedType))
        {
            _handlers[expectedType] = new List<Handler>();
        }

        _handlers[expectedType].Add(handler);

        return handler.Index;
    }

    public void Unsubscribe(EventType type, long idx)
    {
        if (!_handlers.ContainsKey(type))
        {
            return;
        }

        _handlers[type].RemoveAll(handler => handler.Index == idx);
    }
}