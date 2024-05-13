using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Interfaces;

public interface IWorkspaceRepository : IBaseRepository<Workspace>
{ 
    public IEnumerable<WorkspaceImage> GetWorkspaceImages(int workspaceId);
}