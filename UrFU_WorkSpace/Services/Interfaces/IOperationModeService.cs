using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Services.Interfaces;

public interface IOperationModeService
{
    public bool CreateOperationMode(List<WorkspaceWeekday> operationMode, int idWorkspace);

    public IEnumerable<WorkspaceWeekday> ConstructOperationMode(List<(string, string)> jsonOperationMode, int idWorkspace = 0,
        int id = 0);
    
    public IEnumerable<WorkspaceWeekday> GetOperationMode(int idWorkspace);
}