using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Services.Interfaces;

public interface IWorkspaceProvider
{
    public Result<IEnumerable<Workspace>> GetWorkspaces(int? idUser, bool isFull);
    public Result<Workspace> GetWorkspaceById(int idWorkspace, bool isFull);
}