using Microsoft.AspNetCore.JsonPatch;
using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Services.Interfaces;

public interface IWorkspaceService
{
    public Result<IEnumerable<Workspace>> GetWorkspaces(int? idUser, bool isFull);
    public Result<Workspace> GetWorkspaceById(int idWorkspace, bool isFull);
    public Result<IEnumerable<WorkspaceWeekday>> GetOperationMode(int idWorkspace);
    public Result<IEnumerable<WorkspaceAmenity>> GetAmenities(int idWorkspace);
    public Result<IEnumerable<WorkspaceObject>> GetObjects(int idWorkspace);

    public Result<int> CreateWorkspace(ModifyWorkspaceDto modifyWorkspace);

    public Result<Workspace> PutWorkspace(ModifyWorkspaceDto modifyWorkspace, int idWorkspace);
    public Result<None> PatchWorkspace(int idWorkspace, JsonPatchDocument<BaseInfo> workspaceComponent);

    public Result<None> DeleteWorkspace(int id);
}