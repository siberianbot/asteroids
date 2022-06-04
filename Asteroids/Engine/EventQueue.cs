namespace Asteroids.Engine;

public class EventQueue
{
    private List<Event> _pendingEvents = new List<Event>();

    public event Action<Event> Event = delegate { };

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
            Event.Invoke(@event);
        }
    }
}