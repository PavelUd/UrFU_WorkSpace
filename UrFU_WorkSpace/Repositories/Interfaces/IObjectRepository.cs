using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Services.Interfaces;

public interface IObjectRepository
{
    public bool CreateObject(Dictionary<string, object> data);
    public Task<List<WorkspaceObject>> GetWorkspaceObjects(int idWorkspace);
}