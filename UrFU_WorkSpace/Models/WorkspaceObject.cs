namespace UrFU_WorkSpace.Models;

public class WorkspaceObject
{
    public int IdObject { get; set; }
    
    public string Type { get; set; }

    public bool IsReserve;
    
    public double X;
    public double  Y;
    public int Height;
    public int Width;
}