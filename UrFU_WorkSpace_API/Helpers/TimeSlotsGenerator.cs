using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Services;
using UrFU_WorkSpace_API.Services.Interfaces;
using UrFU_WorkSpace_API.Services.WorkspaceComponentsServices;

namespace UrFU_WorkSpace_API.Helpers;

public class TimeSlotsGenerator
{

    private readonly IWorkspaceObjectService _objectService;
    private readonly IWorkspaceComponentService<WorkspaceWeekday> _operationModeService;
    
    public TimeSlotsGenerator(IWorkspaceObjectService objectService, IWorkspaceComponentService<WorkspaceWeekday> operationModeService)
    {
        _objectService = objectService;
        _operationModeService = operationModeService;
    }
    
    
    public List<TimeSlot> GenerateTimeSlots(int idWorkspace, DateOnly date, TimeType timeType)
    {
        var weekday = _operationModeService.GetComponents(idWorkspace)
            .FirstOrDefault(w => w.WeekDayNumber == (int)date.DayOfWeek) ?? new WorkspaceWeekday();
        
        var timeSlots = new List<TimeSlot>();
        var timeStart = weekday.TimeStart;
        var slotsLen = ( weekday.TimeEnd -  weekday.TimeStart).TotalMinutes / (int)timeType;

        for (var i = 0; i < slotsLen; i++)
        {
            var timeEnd = timeStart.AddMinutes((int)timeType);
           
            if (timeEnd > weekday.TimeEnd) continue;
            var anyFree = _objectService.GetComponents(weekday.IdWorkspace,date, timeStart, timeEnd).Any(obj => obj.IsReserved == false);
            timeSlots.Add(new TimeSlot(timeStart, timeEnd, anyFree));
            timeStart = timeEnd;
        }

        return timeSlots;
    }
}