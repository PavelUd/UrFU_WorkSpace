namespace UrFU_WorkSpace.Models;

public class TimeSlot
{
    public TimeOnly TimeStart { get; set; }
    public TimeOnly TimeEnd { get; set; }
    public bool IsEnabled { get; set; }
}