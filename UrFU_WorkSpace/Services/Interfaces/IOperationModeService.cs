using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Services.Interfaces;

public interface IOperationModeService
{
    public bool CreateOperationMode(IFormCollection form, int idWorkspace);

    public List<WorkspaceWeekday> GetOperationMode(int idWorkspace);
}