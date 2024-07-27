using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Services.WorkspaceComponentsServices;

public class WorkspaceAmenitiesService : WorkspaceComponentService<WorkspaceAmenity>
{
    private readonly TemplateService<AmenityTemplate> _templateService;

    public WorkspaceAmenitiesService(IBaseRepository<WorkspaceAmenity> repository,
        TemplateService<AmenityTemplate> templateService, ErrorHandler errorHandler) : base(repository, errorHandler)
    {
        _templateService = templateService;
    }

    public override Result<None> ValidateComponents(IEnumerable<WorkspaceAmenity> components)
    {
        var result = Result.Ok();
        foreach (var component in components)
        {
            result = ValidateParam(_templateService.GetTemplateById(component.IdTemplate).IsSuccess,
                ErrorType.TemplateNotFound, component.IdTemplate);
            if (!result.IsSuccess)
                break;
        }

        return result;
    }
}