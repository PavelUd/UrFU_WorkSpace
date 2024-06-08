using Microsoft.AspNetCore.Mvc;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Controllers.WorkspaceComponents;

[Route("api/workspaces/{idWorkspace}/amenities")]
public class WorkspaceAmenityController : WorkspaceComponentController<WorkspaceAmenity>
{
    public WorkspaceAmenityController(IBaseRepository<WorkspaceAmenity> repository) : base(repository)
    {
    }
}