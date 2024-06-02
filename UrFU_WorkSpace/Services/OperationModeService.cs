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
    
    public bool CreateOperationMode(IFormCollection form, int idWorkspace)
    {
        var operationMode = new List<(string, string)>()
        {
            (form["mondayStart"], form["mondayEnd"]),
            (form["tuesdayStart"], form["tuesdayEnd"]),
            (form["wednesdayStart"], form["wednesdayEnd"]),
            (form["thursdayStart"], form["thursdayEnd"]),
            (form["fridayStart"], form["fridayEnd"]),
            (form["saturdayStart"], form["saturdayEnd"]),
            (form["sundayStart"], form["sundayEnd"]),
        };
        var flag = true;
        for (var i = 0; i < operationMode.Count; i++)
        {
            if (operationMode[i].Item1 == "" || operationMode[i].Item2 == "")
                continue;
            var dayOfWeek = i + 1;
            var dictionary = CreateWeekdayDictionary(idWorkspace, operationMode[i].Item1, operationMode[i].Item2, dayOfWeek);
            flag = Repository.CreateWeekday(dictionary);

        }

        return flag;
    }

    public List<WorkspaceWeekday> GetOperationMode(int idWorkspace)
    {
        return Repository.GetWorkspaceOperationModeAsync(idWorkspace).Result;
    }
    
    private Dictionary<string, object> CreateWeekdayDictionary(int idWorkspace, string timeStart, string timeEnd, int weekDayNumber)
    {
        return new Dictionary<string, object>()
        {
            { "idWorkspace", idWorkspace },
            { "timeStart", timeStart },
            { "timeEnd", timeEnd },
            { "weekDayNumber", weekDayNumber }
        };
    }
}