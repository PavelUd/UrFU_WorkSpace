using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrFU_WorkSpace_API.Models;

[Table("amenities")]
public class AmenityDetail
{
    [Key]
    [Column("amenity_id")]
    public int Id { get; set; }
        
    [Column("name")]
    public string Name { get; set; }
        
    [Column("icon")]
    public string Icon { get; set; }
        
}