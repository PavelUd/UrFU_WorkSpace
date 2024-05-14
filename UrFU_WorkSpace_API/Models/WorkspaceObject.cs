using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrFU_WorkSpace_API.Models;
[Table("workspace_objects")]
public class WorkspaceObject  
{
    [Key]
    [Column("id_object")]
    public int IdObject { get; set; }
    
    [Column("type")]
    public string Type { get; set; }
    
    [Column("id_workspace")]
    public int IdWorkspace { get; set; }
}