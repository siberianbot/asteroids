namespace Asteroids.Engine;

public class EventQueue
{
    public delegate void Handler(Event @event);

    private readonly Dictionary<EventType, Handler> _handlers = new Dictionary<EventType, Handler>();
    private List<Event> _pendingEvents = new List<Event>();

    public void Push(Event @event)
    {
        _pendingEvents.Add(@event);
    }

    public void ExecutePending()
    {
        List<Event> events = _pendingEvents;

        _pendingEvents = new List<Event>();

        foreach (Event @event in events)
        {
            if (!_handlers.ContainsKey(@event.EventType))
            {
                continue;
            }

            _handlers[@event.EventType](@event);
        }
    }

    public void Reset()
    {
        _pendingEvents.Clear();
        _handlers.Clear();
    }

    public void OnEvent(EventType expectedType, Handler handler)
    {
        _handlers[expectedType] = _handlers.ContainsKey(expectedType)
            ? (Handler)Delegate.Combine(_handlers[expectedType], handler)
            : handler;
    }
}