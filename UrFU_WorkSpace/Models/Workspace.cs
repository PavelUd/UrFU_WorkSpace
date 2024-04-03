namespace UrFU_WorkSpace.Models;

public class Workspace
{
    public int WorkspaceId { get; set; }
    
    public string Description { get; set; }
    
    public double Rating { get; set; }
    
    public string Address { get; set; }
    
    public string Privacy { get; set; }
    
    public int CreatorId { get; set; }
}