using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Services.Interfaces;

public interface IWorkspaceRepository
{

    public Task<int> CreateWorkspaceAsync(Workspace baseInfo);
    public Task<Workspace> GetWorkspaceAsync(int idWorkspace);
}