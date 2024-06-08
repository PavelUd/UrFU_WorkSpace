using System.ComponentModel.DataAnnotations.Schema;
using UrFU_WorkSpace_API.Interfaces;

namespace UrFU_WorkSpace_API.Models;

[Table("workspace_amenities")]
public class WorkspaceAmenity : IWorkspaceComponent
{
    [Column("amenity_id")]
    public int Id { get; set; }
    
    [Column("template_id")]
    public int IdTemplate { get; set; }
    
    [Column("workspace_id")]
    public int IdWorkspace { get; set; }
    
    public AmenityTemplate Template { get; set; }
}