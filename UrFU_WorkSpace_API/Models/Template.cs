using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UrFU_WorkSpace_API.Enums;

namespace UrFU_WorkSpace_API.Models;

[Table("templates")]
public class Template : IModel
{
    public TemplateImage Image;

    [Column("name")] 
    public string Name { get; set; }

    [Column("category")] 
    public TemplateCategory Category { get; set; }

    [Key] [Column("template_id")] 
    public int Id { get; set; }
}