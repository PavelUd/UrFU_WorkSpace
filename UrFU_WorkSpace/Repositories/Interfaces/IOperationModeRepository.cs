using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Services.Interfaces;

public interface IOperationModeRepository
{
    public Task<List<WorkspaceWeekday>> GetWorkspaceOperationModeAsync(int idWorkspace);
    public bool CreateWeekday(WorkspaceWeekday weekday);
}