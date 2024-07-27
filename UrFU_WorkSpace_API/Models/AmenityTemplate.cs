using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrFU_WorkSpace_API.Models;

[Table("amenity_templates")]
public class AmenityTemplate : IModel
{
    public AmenityImage Image;

    [Column("name")] public string Name { get; set; }

    [Column("category")] public string Category { get; set; }

    [Key] [Column("template_id")] public int Id { get; set; }
}