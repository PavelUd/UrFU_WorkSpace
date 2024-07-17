using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Services.WorkspaceComponentsServices;

public class WorkspaceAmenitiesService : WorkspaceComponentService<WorkspaceAmenity>
{
    private TemplateService<AmenityTemplate>  TemplateService;
    
    public WorkspaceAmenitiesService(IBaseRepository<WorkspaceAmenity> repository, TemplateService<AmenityTemplate> templateService) : base(repository)
    {
        TemplateService = templateService;
    }
    public override Result<None> ValidateComponents(IEnumerable<WorkspaceAmenity> components)
    {
        var result = Result.Ok();
        foreach (var component in components)
        {
            result = ValidateParam(TemplateService.GetTemplateById(component.IdTemplate).IsSuccess,
                ErrorType.TemplateNotFound, component.IdTemplate);
            if (!result.IsSuccess)
                break;
        }

        return result;
    }
}