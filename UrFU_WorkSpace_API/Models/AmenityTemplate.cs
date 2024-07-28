using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UrFU_WorkSpace_API.Models;

[Table("amenity_templates")]
public class AmenityTemplate
{
    [Key]
    [Column("template_id")]
    public int Id { get; set; }
    
    [Column("picture")] public string Picture { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
        
    [Column("category")]
    public string Category { get; set; }
    
    [JsonIgnore]
    public ICollection<WorkspaceAmenity>? WorkspaceAmenities { get; set; }
    
        
}