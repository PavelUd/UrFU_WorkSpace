using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Helpers.Events;

public class WorkspaceUpdatedEvent
{
    public WorkspaceUpdatedEvent(IEnumerable<WorkspaceWeekday> weekdays, IEnumerable<WorkspaceObject> objects)
    {
        Weekdays = weekdays;
        Objects = objects;
    }

    public IEnumerable<WorkspaceWeekday> Weekdays { get; }
    public IEnumerable<WorkspaceObject> Objects { get; }
}