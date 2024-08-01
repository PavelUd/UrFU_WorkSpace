using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Models;

namespace UrFU_WorkSpace.Services.Interfaces;

public interface IOperationModeService
{
    public IEnumerable<WorkspaceWeekday> ConstructOperationMode(List<(string, string)> jsonOperationMode,
        int idWorkspace = 0, int id = 0);
}