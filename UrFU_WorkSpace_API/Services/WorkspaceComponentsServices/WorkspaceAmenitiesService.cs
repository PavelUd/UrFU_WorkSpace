using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Services.Interfaces;

namespace UrFU_WorkSpace_API.Services.WorkspaceComponentsServices;

public class WorkspaceAmenitiesService : WorkspaceComponentService<WorkspaceAmenity>
{
    private readonly IBaseProvider<Template> _templateProvider;
    public WorkspaceAmenitiesService(IBaseRepository<WorkspaceAmenity> repository,IBaseProvider<Template> templateProvider, ErrorHandler errorHandler) : base(repository, errorHandler)
    {
        _templateProvider = templateProvider;
    }

    public override Result<None> ValidateComponents(IEnumerable<WorkspaceAmenity> components)
    {
        var result = Result.Ok();
        foreach (var component in components)
        {
            var template = _templateProvider.FindByCondition(t => t.Id == component.Id && t.Category == TemplateCategory.Amenity).FirstOrDefault();
            
            result = ValidateParam(template != null, ErrorType.TemplateNotFound, component.Id);
            if (!result.IsSuccess)
                break;
        }

        return result;
    }
}