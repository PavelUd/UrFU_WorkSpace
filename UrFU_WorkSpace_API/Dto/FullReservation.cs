using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Dto;

public class FullReservation
{
    public int IdUser { get; set; }
    
    public TimeOnly TimeStart { get; set; }
    
    public TimeOnly TimeEnd { get; set; }

    public Coordinate Coordinate;
    public DateOnly Date { get; set; }
    public int IdObject { get; set; }
    public WorkspaceObject WorkspaceObject { get; set; }
    
    public bool IsConfirmed { get; set; }
    
    public bool IsAvailableToConfirm { get; set; }
    public WorkspaceImage? Image { get; set; }
    
    public string WorkspaceName { get; set; }
    
    public int Id { get; set; }
    
   public FullReservation()
   {
   }

    public FullReservation(Reservation reservation, Workspace workspace, WorkspaceObject workspaceObject)
    {
        Coordinate = new Coordinate(workspace.Latitude, workspace.Longitude);
        WorkspaceName = workspace.Name;
        Image = workspace.Images.FirstOrDefault();
        IsConfirmed = reservation.IsConfirmed;
        IdUser = reservation.IdUser;
        TimeStart = reservation.TimeStart;
        WorkspaceObject = workspaceObject;
        TimeEnd = reservation.TimeEnd;
        Id = reservation.Id;
        Date = reservation.Date;
        IsAvailableToConfirm = IsAvailable(reservation) && !IsConfirmed;
    }
    
    private static bool IsAvailable(Reservation reservation)
    {
        var date = DateOnly.FromDateTime(DateTime.Now);
        var time = TimeOnly.FromDateTime(DateTime.Now);
        const int minutes = 10;
        return date == reservation.Date && (reservation.TimeStart.AddMinutes(-minutes) <= time) && reservation.TimeEnd.AddMinutes(minutes) >= time;
    }
}