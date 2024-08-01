namespace UrFU_WorkSpace_API.Models;

public class TimeSlot
{
    public TimeOnly TimeStart;
    public TimeOnly TimeEnd;
    public bool IsEnabled;
    public TimeSlot(TimeOnly timeStart, TimeOnly timeEnd,bool isEnabled = true)
    {
        this.TimeStart = timeStart;
        this.TimeEnd = timeEnd;
        this.IsEnabled = isEnabled;
    }
}