using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrFU_WorkSpace_API.Models;

[Table("object_templates")]
public class ObjectTemplate : IModel
{
    public ObjectImage Image;

    [Column("category")] public string Category { get; set; }

    [Column("isReservable")] public bool IsReservable { get; set; }
    [Key] [Column("template_id")] public int Id { get; set; }
}