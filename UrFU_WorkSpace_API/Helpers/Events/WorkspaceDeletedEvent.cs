namespace UrFU_WorkSpace_API.Helpers.Events;

public class WorkspaceDeletedEvent
{
    public WorkspaceDeletedEvent(int id)
    {
        WorkspaceId = id;
    }

    public int WorkspaceId { get; }
}