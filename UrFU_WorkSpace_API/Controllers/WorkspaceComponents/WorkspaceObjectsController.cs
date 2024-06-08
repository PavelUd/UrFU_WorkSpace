using Microsoft.AspNetCore.Mvc;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Controllers;

[Route("api/workspaces/{idWorkspace}/objects")]
public class WorkspaceObjectsController : WorkspaceComponentController<WorkspaceObject>
{
    public WorkspaceObjectsController(IBaseRepository<WorkspaceObject> repository) : base(repository)
    {
    }
}