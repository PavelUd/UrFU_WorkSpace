using System.Collections;
using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Repository;
using UrFU_WorkSpace_API.Services.Interfaces;

namespace UrFU_WorkSpace_API.Services.WorkspaceComponentsServices;

public class WorkspaceObjectsService(IBaseRepository<WorkspaceObject> repository, ErrorHandler errorHandler, IBaseProvider<Template> templateProvider, IReservationProvider reservationProvider)  : WorkspaceComponentService<WorkspaceObject>(repository, errorHandler), IWorkspaceObjectService
{
    public override Result<None> ValidateComponents(IEnumerable<WorkspaceObject> components)
    {
        var result = Result.Ok();
        foreach (var component in components)
        {
            var template = templateProvider.FindByCondition(t => t.Id == component.IdTemplate && t.Category == TemplateCategory.Object).FirstOrDefault();
            result = ValidateParam(template != null, ErrorType.TemplateNotFound, component.IdTemplate)
                .Then(_ => ValidateParam(component.X > 0, ErrorType.InvalidPosition, component.X, "X"))
                .Then(_ => ValidateParam(component.Y > 0, ErrorType.InvalidPosition, component.Y, "Y"))
                .Then(_ => ValidateParam(component.Width >= 1, ErrorType.InvalidSize, component.Width, "Ширина"))
                .Then(_ => ValidateParam(component.Height >= 1, ErrorType.InvalidSize, component.Height, "Высота"));
            if (!result.IsSuccess)
                break;
        }

        return result;
    }

    public IEnumerable<WorkspaceObject> GetComponents(int idWorkspace, DateOnly date, TimeOnly timeStart, TimeOnly timeEnd)
    {
        var reservedObjectIds = GetReservedObjectIds(idWorkspace, date, timeStart, timeEnd);
        var objects = GetComponents(idWorkspace).ToArray();
        foreach (var obj in objects)
        {
            obj.IsReserved = reservedObjectIds.Contains(obj.Id);
        }

        return objects;
    }

    public IEnumerable<WorkspaceObject> FilterByTemplate(IEnumerable<WorkspaceObject> objects, int idTemplate)
    {
        return objects.Where(obj => obj.IdTemplate == idTemplate || idTemplate == 0);
    }

    private HashSet<int> GetReservedObjectIds(int idWorkspace, DateOnly date, TimeOnly timeStart, TimeOnly timeEnd)
    {
        return reservationProvider.GetWorkspaceReservationByTimeSlot(idWorkspace, date, timeStart, timeEnd).Select(r => r.IdObject).ToHashSet();
    }
}