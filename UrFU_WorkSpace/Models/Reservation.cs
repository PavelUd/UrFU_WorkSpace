namespace UrFU_WorkSpace.Models;

public class Reservation
{
    public int IdUser { get; set; }
    
    public TimeOnly TimeStart { get; set; }
    
    public TimeOnly TimeEnd { get; set; }
    
    public int IdObject { get; set; }
    
    public bool IsAvailableToConfirm { get; set; }
    
    public DateOnly Date { get; set; }
    
    public WorkspaceObject WorkspaceObject { get; set; }
    
    public bool IsConfirmed { get; set; }
    
    public Image Image { get; set; }
    
    public string WorkspaceName { get; set; }
    
    public int Id { get; set; }
}