using Microsoft.AspNetCore.Mvc;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Controllers.WorkspaceComponents;

[Route("api/workspaces/{idWorkspace}/operation-mode")]
public class OperationModeController : WorkspaceComponentController<WorkspaceWeekday>
{
    public OperationModeController(IBaseRepository<WorkspaceWeekday> repository) : base(repository)
    {
    }
}