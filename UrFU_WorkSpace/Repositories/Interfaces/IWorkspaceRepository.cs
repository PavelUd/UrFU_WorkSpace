
using UrFU_WorkSpace.enums;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Repositories.Interfaces;

public interface IWorkspaceRepository
{
    public Task<Result<int>> CreateWorkspaceAsync(Workspace baseInfo, string token);
    public Task<Result<Workspace>> GetWorkspaceAsync(int idWorkspace, bool isFull);
    public Task<Result<List<TimeSlot>>> GetTimeSlots(int idWorkspace, DateOnly date, TimeType timeType, int? idObjectTemplate);
    public Task<Result<List<WorkspaceObject>>> FetchWorkspaceObjectsByDateRange(int idWorkspace,
        int? idTemplate, DateOnly? date, TimeOnly? timeStart, TimeOnly? timeEnd);
    public Task<Result<List<Workspace>>> GetAllWorkspacesAsync();
}