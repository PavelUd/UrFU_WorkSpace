using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrFU_WorkSpace_API.Models;
[Table("workspace_objects")]
public class WorkspaceObject  
{
    [Key]
    [Column("object_id")]
    public int IdObject { get; set; }
    
    [Column("workspace_id")]
    public int WorkspaceId { get; set; }
    
    [Column("template_id")]
    public int IdTemplate { get; set; }
    
    [Column("pos_x")]
    public double X { get; set; }
    
    [Column("pos_y")]
    public double  Y { get; set; }
    
    [Column("height")]
    public int Height { get; set; }
    
    [Column("width")]
    public int Width { get; set; }

    public ObjectTemplate Template { get; set; }
}