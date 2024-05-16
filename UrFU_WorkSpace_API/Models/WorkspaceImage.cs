using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrFU_WorkSpace_API.Models;

[Table("workspace_images")]
public class WorkspaceImage
{
    [Key]
    [Column("id_image")] 
    public int Id { get; set; }

    [Column("url")]
    public string Url { get; set; }
    
    [Column("id_workspace")]
    public int IdWorkspace { get; set; }
}