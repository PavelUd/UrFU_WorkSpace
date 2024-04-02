using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Interfaces;

public interface IWorkspaceRepository
{ 
    public Workspace GetWorkspace(int workspaceId);
    public IEnumerable<Workspace> GetWorkspaces();
}