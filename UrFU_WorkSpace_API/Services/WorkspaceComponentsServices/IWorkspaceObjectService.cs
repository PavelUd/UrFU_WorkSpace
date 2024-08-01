using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Services.WorkspaceComponentsServices;

public interface IWorkspaceObjectService : IWorkspaceComponentService<WorkspaceObject>
{
    public IEnumerable<WorkspaceObject> GetComponents(int idWorkspace, DateOnly date, TimeOnly timeStart,
        TimeOnly timeEnd);

    public IEnumerable<WorkspaceObject> FilterByTemplate(IEnumerable<WorkspaceObject> objects, int idTemplate);
}