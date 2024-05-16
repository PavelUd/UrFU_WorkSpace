using System.ComponentModel.DataAnnotations.Schema;

namespace UrFU_WorkSpace_API.Models;

[Table("workspace_amenities")]
public class WorkspaceAmenity
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("amenity_id")]
    public int IdAmenity { get; set; }
    
    [Column("workspace_id")]
    public int IdWorkspace { get; set; }

    public virtual AmenityDetail Detail { get; set; }
}