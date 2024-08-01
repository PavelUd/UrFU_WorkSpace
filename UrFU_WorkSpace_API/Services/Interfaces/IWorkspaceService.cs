using Microsoft.AspNetCore.JsonPatch;
using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Services.Interfaces;

public interface IWorkspaceService
{
    public Result<IEnumerable<Workspace>> GetWorkspaces(int? idUser, bool isFull);
    public Result<Workspace> GetWorkspaceById(int idWorkspace, bool isFull);
    public Result<IEnumerable<WorkspaceWeekday>> GetOperationMode(int idWorkspace);
    public Result<IEnumerable<WorkspaceAmenity>> GetAmenities(int idWorkspace);
    public Result<IEnumerable<WorkspaceObject>> GetObjects(int idWorkspace, DateOnly date, TimeOnly timeStart,
        TimeOnly timeEnd, int idTemplate);

    public Result<IEnumerable<TimeSlot>> GetTimeSlots(int idWorkspace, DateOnly dateOnly, TimeType type);
    public Result<int> CreateWorkspace(ModifyWorkspaceDto modifyWorkspace);

    public Result<Workspace> PutWorkspace(ModifyWorkspaceDto modifyWorkspace, int idWorkspace);
    public Result<None> UpdateRating(int idWorkspace);

    public Result<None> DeleteWorkspace(int id);
}