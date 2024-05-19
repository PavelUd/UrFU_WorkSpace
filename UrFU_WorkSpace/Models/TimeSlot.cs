namespace UrFU_WorkSpace.Models;

public class TimeSlot
{
    public TimeOnly TimeStart;
    public TimeOnly TimeEnd;
    public bool IsDisable;
    public TimeSlot(TimeOnly timeStart, TimeOnly timeEnd,bool isDisable = false)
    {
        this.TimeStart = timeStart;
        this.TimeEnd = timeEnd;
        this.IsDisable = isDisable;
    }
}