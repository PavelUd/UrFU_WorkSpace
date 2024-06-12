using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Interfaces;

public interface IWorkspaceRepository : IBaseRepository<Workspace>
{ 
    public IEnumerable<Image> GetWorkspaceImages(int workspaceId);
    public IEnumerable<WorkspaceObject> GetWorkspaceObjects(int workspaceId);

    public IEnumerable<WorkspaceAmenity> GetWorkspaceAmenities(int workspaceId);
    public IEnumerable<WorkspaceWeekday> GetWorkspaceOperationMode(int workspaceId);
}