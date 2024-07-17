using Microsoft.AspNetCore.Http.HttpResults;
using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Services.WorkspaceComponentsServices;

public class WorkspaceObjectsService : WorkspaceComponentService<WorkspaceObject>
{
    private TemplateService<ObjectTemplate>  TemplateService;
    
    public WorkspaceObjectsService(IBaseRepository<WorkspaceObject> repository, TemplateService<ObjectTemplate> templateService) : base(repository)
    {
        TemplateService = templateService;
    }
    
    public override Result<None> ValidateComponents(IEnumerable<WorkspaceObject> components)
    {
        var result = Result.Ok();
        foreach (var component in components)
        {
            result = ValidateParam(component.X > 0, ErrorType.InvalidPosition, component.X, "X")
                .Then(_ => ValidateParam(component.Y > 0, ErrorType.InvalidPosition, component.Y, "Y"))
                .Then(_ => ValidateParam(component.Width >= 1, ErrorType.InvalidSize, component.Width, "Ширина"))
                .Then(_ => ValidateParam(component.Height >= 1, ErrorType.InvalidSize, component.Height, "Высота"))
                .Then(_ => ValidateParam(TemplateService.GetTemplateById(component.IdTemplate).IsSuccess, ErrorType.TemplateNotFound, component.IdTemplate));
            if (!result.IsSuccess)
                break;
        }

        return result;
    }
}