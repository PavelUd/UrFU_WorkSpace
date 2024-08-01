using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Services.Interfaces;

public interface IWorkspaceService
{
    public Task<Result<Workspace>> GetWorkspace(int idWorkspace);
    public Task<Result<List<Workspace>>> GetAllWorkspaces();

    public Task<Result<List<WorkspaceObject>>> GetWorkspaceObjects(int idWorkspace, int? idTemplate, DateOnly? date,
        TimeOnly? timeStart, TimeOnly? timeEnd);
    public Task<Result<List<TimeSlot>>> GetWorkspaceTimeSlots(int idWorkspace, DateTime date, TimeType timeType, int idTemplate);
}