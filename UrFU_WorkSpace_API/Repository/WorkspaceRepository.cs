using UrFU_WorkSpace_API.Context;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Repository;

public class WorkspaceRepository : IWorkspaceRepository
{
    private readonly UrfuWorkSpaceContext Context;
    public WorkspaceRepository(UrfuWorkSpaceContext context)
    {
        Context = context;
    }
    
    public Workspace GetWorkspace(int workspaceId)
    {
        return Context.Workspaces.Find(workspaceId);
    }

    public IEnumerable<Workspace> GetWorkspaces()
    {
        return Context.Workspaces;
    }
}