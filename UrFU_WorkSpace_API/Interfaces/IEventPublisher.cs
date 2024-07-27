namespace UrFU_WorkSpace_API.Interfaces;

public interface IEventPublisher
{
    void Publish<TEvent>(TEvent @event);
    public void Subscribe<TEvent>(IEventHandler<TEvent> handler);
}