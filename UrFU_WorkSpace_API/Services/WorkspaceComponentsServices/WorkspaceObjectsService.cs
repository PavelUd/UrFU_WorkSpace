using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Services.Interfaces;

namespace UrFU_WorkSpace_API.Services.WorkspaceComponentsServices;

public class WorkspaceObjectsService(IBaseRepository<WorkspaceObject> repository, ErrorHandler errorHandler, IBaseProvider<Template> templateProvider)  : WorkspaceComponentService<WorkspaceObject>(repository, errorHandler)
{
    private readonly IBaseProvider<Template> _templateProvider = templateProvider;
    public override Result<None> ValidateComponents(IEnumerable<WorkspaceObject> components)
    {
        var result = Result.Ok();
        foreach (var component in components)
        {
            var template = _templateProvider.FindByCondition(t => t.Id == component.Id && t.Category == TemplateCategory.Object).FirstOrDefault();
            result = ValidateParam(template != null, ErrorType.TemplateNotFound, component.Id)
                .Then(_ => ValidateParam(component.X > 0, ErrorType.InvalidPosition, component.X, "X"))
                .Then(_ => ValidateParam(component.Y > 0, ErrorType.InvalidPosition, component.Y, "Y"))
                .Then(_ => ValidateParam(component.Width >= 1, ErrorType.InvalidSize, component.Width, "Ширина"))
                .Then(_ => ValidateParam(component.Height >= 1, ErrorType.InvalidSize, component.Height, "Высота"));
            if (!result.IsSuccess)
                break;
        }

        return result;
    }
}