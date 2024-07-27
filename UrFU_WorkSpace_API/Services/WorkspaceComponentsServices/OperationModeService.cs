using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Services.WorkspaceComponentsServices;

namespace UrFU_WorkSpace_API.Services;

public class OperationModeService : WorkspaceComponentService<WorkspaceWeekday>
{
    public OperationModeService(IBaseRepository<WorkspaceWeekday> repository, ErrorHandler errorHandler) : base(repository, errorHandler)
    {
    }

    public override Result<None> ValidateComponents(IEnumerable<WorkspaceWeekday> components)
    {
        var result = ValidateParam<None>(components.Count() < 7, ErrorType.IncorrectCountWeekdays);
        foreach (var component in components)
        {
            result = ValidateParam(component.WeekDayNumber < 7, ErrorType.InvalidDayOfWeek, component.WeekDayNumber);
            if (!result.IsSuccess)
                break;
        }

        return result;
    }
}