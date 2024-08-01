namespace UrFU_WorkSpace.Models;

public class WorkspaceObject
{
    public int Id { get; set; }
    
    public int IdWorkspace { get; set; }
    
    public int IdTemplate { get; set; }
    
    public double X { get; set; }
    
    public double  Y { get; set; }
    
    public int Height { get; set; }
    public bool isReserved { get; set; }
    public int Width { get; set; }

    public Template? Template { get; set; }
}