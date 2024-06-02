using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Services.Interfaces;

public interface IObjectService
{
    public bool CreateObjects(int idWorkspace, string jsonObjects);
    public Task<List<WorkspaceObject>> GetWorkspaceObjects(int idWorkspace);
    
    
}