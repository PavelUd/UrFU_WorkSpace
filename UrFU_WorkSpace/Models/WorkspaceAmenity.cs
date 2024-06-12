namespace UrFU_WorkSpace.Models;

public class WorkspaceAmenity
{
    public int Id { get; set; }
    
    public int IdTemplate { get; set; }
    
    public int IdWorkspace { get; set; }
    
    public AmenityTemplate Template { get; set; }
}