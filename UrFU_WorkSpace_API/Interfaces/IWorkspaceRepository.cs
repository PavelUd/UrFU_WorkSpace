using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Interfaces;

public interface IWorkspaceRepository
{ 
    public Workspace GetWorkspaceById(int workspaceId);
    public IEnumerable<Workspace> GetWorkspaces();
    public IEnumerable<WorkspaceImage> GetWorkspaceImages(int workspaceId);
}