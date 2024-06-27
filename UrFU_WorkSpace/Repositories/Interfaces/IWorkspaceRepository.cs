using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Services.Interfaces;

public interface IWorkspaceRepository
{

    public int CreateWorkspaceAsync(Workspace baseInfo);
    public Task<Workspace> GetWorkspaceAsync(int idWorkspace);

    public Task<int> UpdateRating(double rating, int idWorkspace);

    public Task<List<Workspace>> GetAllWorkspacesAsync();
}