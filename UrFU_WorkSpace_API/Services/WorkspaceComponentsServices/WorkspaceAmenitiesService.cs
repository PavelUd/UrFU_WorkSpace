using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Services.WorkspaceComponentsServices;

public class WorkspaceAmenitiesService : WorkspaceComponentService<WorkspaceAmenity>
{

    public WorkspaceAmenitiesService(IBaseRepository<WorkspaceAmenity> repository, ErrorHandler errorHandler) : base(repository, errorHandler)
    {
    }

    public override Result<None> ValidateComponents(IEnumerable<WorkspaceAmenity> components)
    {
        return Result.Ok();
    }
}