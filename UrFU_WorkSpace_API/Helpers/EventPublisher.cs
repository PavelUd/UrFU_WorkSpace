using UrFU_WorkSpace_API.Interfaces;

namespace UrFU_WorkSpace_API.Helpers;

public class EventPublisher : IEventPublisher
{
    private readonly Dictionary<Type, List<object>> _handlers = new();

    public void Publish<TEvent>(TEvent @event)
    {
        var eventType = typeof(TEvent);
        if (!_handlers.ContainsKey(eventType))
            return;

        foreach (var handler in _handlers[eventType].Cast<IEventHandler<TEvent>>()) handler.Handle(@event);
    }

    public void Subscribe<TEvent>(IEventHandler<TEvent> handler)
    {
        var eventType = typeof(TEvent);
        if (!_handlers.ContainsKey(eventType)) _handlers[eventType] = new List<object>();
        _handlers[eventType].Add(handler);
    }
}