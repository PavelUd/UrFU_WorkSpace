using UrFU_WorkSpace_API.Helpers;

namespace UrFU_WorkSpace_API.Interfaces;

public interface IEventHandler<TEvent>
{
    Result<None> Handle(TEvent @event);
}