using UrFU_WorkSpace.Models;
using UrFU_WorkSpace.Services.Interfaces;

namespace UrFU_WorkSpace.Services;

public class OperationModeService : IOperationModeService
{
    private IOperationModeRepository Repository;
    
    public OperationModeService(IOperationModeRepository repository)
    {
        Repository = repository;
    }

    public bool CreateOperationMode(List<WorkspaceWeekday> operationMode, int idWorkspace)
    {
        var flag = true;
        foreach (var weekday in operationMode)
        {
            flag = Repository.CreateWeekday(weekday);
        }

        return flag;
    }

    public IEnumerable<WorkspaceWeekday> ConstructOperationMode(List<(string, string)> jsonOperationMode, int idWorkspace = 0, int id = 0)
    {
        var operationMode = new List<WorkspaceWeekday>();
        for (var i = 0; i < jsonOperationMode.Count; i++)
        {
            if (jsonOperationMode[i].Item1 == "" || jsonOperationMode[i].Item2 == "")
                continue;
            var dayOfWeek = i + 1;
            var weekday = CreateWeekday(idWorkspace, jsonOperationMode[i].Item1, jsonOperationMode[i].Item2, dayOfWeek, id);
            operationMode.Add(weekday);
        }

        return operationMode;
    }

    public IEnumerable<WorkspaceWeekday> GetOperationMode(int idWorkspace)
    {
        return Repository.GetWorkspaceOperationModeAsync(idWorkspace).Result;
    }
    
    private static WorkspaceWeekday CreateWeekday(int idWorkspace, string timeStart, string timeEnd, int weekDayNumber, int id)
    {
        return new WorkspaceWeekday
        {
            Id = id,
            IdWorkspace = idWorkspace,
            TimeStart = JsonHelper.Deserialize<TimeOnly>(timeStart),
            TimeEnd = JsonHelper.Deserialize<TimeOnly>(timeEnd),
            WeekDayNumber = weekDayNumber
        };
    }
}