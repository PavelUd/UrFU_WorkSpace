using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using UrFU_WorkSpace_API.Interfaces;

namespace UrFU_WorkSpace_API.Models;

[Table("amenity_templates")]
public class AmenityTemplate : IModel
{
    [Key]
    [Column("template_id")]
    public int Id { get; set; }

    public AmenityImage Image;
    
    [Column("name")]
    public string Name { get; set; }
        
    [Column("category")]
    public string Category { get; set; }
    
}