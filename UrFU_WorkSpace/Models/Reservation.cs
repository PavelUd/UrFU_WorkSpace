namespace UrFU_WorkSpace.Models;

public class Reservation
{
    public int IdReservation { get; set; }
    public int IdObject { get; set; }
    
    public int IdUser { get; set; }
    
    public int IdWorkspace { get; set; }
    public TimeOnly TimeStart { get; set; }
    public TimeOnly TimeEnd { get; set; }
    
    public DateOnly Date { get; set; }
    
    public bool IsConfirmed { get; set; }
}