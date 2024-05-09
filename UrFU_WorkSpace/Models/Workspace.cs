using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace.Models;

public class Workspace
{
    public int IdWorkspace { get; set; }
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public double Rating { get; set; }
    
    public string Address { get; set; }
    
    public string Institute { get; set; }
    
    public int Privacy { get; set; }
    
    public int CreatorId { get; set; }
    
    public IEnumerable<WorkspaceImage> Images { get; set; }
}