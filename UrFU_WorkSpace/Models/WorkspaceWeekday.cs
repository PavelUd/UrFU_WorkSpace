using Newtonsoft.Json;
using UrFU_WorkSpace.Helpers;

namespace UrFU_WorkSpace.Models;

public class WorkspaceWeekday
{
    public int Id { get; set; }
    
    public int IdWorkspace { get; set; }
    public TimeOnly TimeStart { get; set; }
    
    public TimeOnly TimeEnd { get; set; }
    
    public int WeekDayNumber { get; set; }
    
}