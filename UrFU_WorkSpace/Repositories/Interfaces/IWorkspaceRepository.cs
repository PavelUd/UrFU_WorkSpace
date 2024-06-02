using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Services.Interfaces;

public interface IWorkspaceRepository
{

    public Task<int> CreateWorkspaceAsync(Dictionary<string, object> baseInfo);
    public Task<Workspace> GetWorkspaceAsync(int idWorkspace);
    public  Task<bool> SaveWorkspace();
}