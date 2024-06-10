using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Services.Interfaces;

public interface IWorkspaceRepository
{

    public Task<bool> CreateWorkspaceAsync(Workspace baseInfo);
    public Task<Workspace> GetWorkspaceAsync(int idWorkspace);
}