using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Services.Interfaces;

public interface IObjectRepository
{

    public Task<IEnumerable<ObjectTemplate>> GetObjectTemplates();
    public bool CreateObject(WorkspaceObject data);
    public Task<List<WorkspaceObject>> GetWorkspaceObjects(int idWorkspace);
}